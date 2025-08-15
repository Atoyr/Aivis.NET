using System.Diagnostics;

using OpenTK.Audio.OpenAL;

namespace Aivis.Speakers;

/// <summary>
/// MP3形式の音声をOpenALで再生するスピーカー
/// </summary>
public class MP3Speaker : ISpeaker, IDisposable
{
    // OpenAL
    private ALDevice _device;
    private ALContext _context;
    private int _source;
    private int[] _buffers = Array.Empty<int>();

    // パイプライン
    private Process? _ffmpeg;
    private Task? _pumpInTask;
    private volatile bool _running;

    // 設定
    private readonly FFmpegDecodeOptions _opt;
    private readonly int _bufferCount;
    private readonly int _bytesPerSample = 2; // s16
    private readonly int _bufferMillis;


    // 再生時の並列処理制御用
    // FFmpegプロデューサ終了通知（Stopはしない）
    private volatile bool _producerExited;
    // 再生専用スレッド
    private Thread? _audioThread;
    // 再生完了通知（PlayAsyncで待つ）
    private TaskCompletionSource<bool> _playbackTcs =
        new(TaskCreationOptions.RunContinuationsAsynchronously);
    // 完了ヘルパ
    private void CompleteOnceSuccess() => _playbackTcs.TrySetResult(true);
    private void CompleteOnceError(Exception ex) => _playbackTcs.TrySetException(ex);

    /// <summary>
    /// 音量制御
    /// </summary>
    public float Volume
    {
        get { AL.GetSource(_source, ALSourcef.Gain, out float g); return g; }
        set { AL.Source(_source, ALSourcef.Gain, Math.Clamp(value, 0f, 1f)); }
    }

    public MP3Speaker(int bufferCount = 4, int bufferMillis = 100)
    {
        _opt = new FFmpegDecodeOptions();
        _bufferCount = Math.Max(1, bufferCount);
        _bufferMillis = Math.Max(10, bufferMillis);
    }

    private void InitializeOpenAL()
    {
        // OpenAL 初期化
        _device = ALC.OpenDevice(null);
        if (_device == IntPtr.Zero) throw new InvalidOperationException("OpenAL device を開けません。");
        _context = ALC.CreateContext(_device, Array.Empty<int>());
        if (_context == ALContext.Null) throw new InvalidOperationException("OpenAL context 作成に失敗。");
        ALC.MakeContextCurrent(_context);

        _source = AL.GenSource();
        _buffers = new int[_bufferCount];
        AL.GenBuffers(_buffers);
        CheckALError();
    }

    /// <summary>
    /// FFmpegのデコードオプションを設定します。
    /// </summary>
    public void ConfigureBuildOptions(Action<FFmpegDecodeOptions> configure) => configure(_opt);

    /// <inheritdoc />
    public async Task PlayAsync(Stream audioStream, CancellationToken cancellationToken = default)
    {
        if (_running) throw new InvalidOperationException("すでに再生中です。");
        _running = true;

        _playbackTcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
        _producerExited = false;

        // FFmpeg 起動（stdin:エンコード済み, stdout:PCM s16le）
        _ffmpeg = FFmpegProcessFactory.CreateProcess(_opt);
        if (!_ffmpeg.Start()) throw new InvalidOperationException("ffmpeg起動に失敗しました。");

        // FFmpeg stderr（任意でログ）
        _ = Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    var line = await _ffmpeg.StandardError.ReadLineAsync();
                    if (line == null)
                    {
                        break;
                    }
                    Console.Error.WriteLine("[ffmpeg] " + line);
                }
            }
            catch { /* ignore */ }
        });

        // 入力ポンプ（エンコード済み→ffmpeg stdin）
        var stdin = _ffmpeg.StandardInput.BaseStream;
        _pumpInTask = Task.Run(async () =>
        {
            var buf = new byte[8192];
            try
            {
                int n;
                while ((n = await audioStream.ReadAsync(buf.AsMemory(0, buf.Length), cancellationToken)) > 0)
                {
                    await stdin.WriteAsync(buf.AsMemory(0, n), cancellationToken);
                    await stdin.FlushAsync(cancellationToken);
                }
            }
            catch { /* cancel/EOF */ }
            finally
            {
                try
                {
                    stdin.Close();
                }
                catch { }
            }
        }, cancellationToken);

        // 背景でFFmpeg終了監視（先に終わったら再生も止める）
        _ = Task.Run(async () =>
        {
            try { await _ffmpeg.WaitForExitAsync(); }
            catch { }
            finally { _producerExited = true; }
        });

        // 出力読み取り→OpenAL 再生ループ
        var pcm = _ffmpeg.StandardOutput.BaseStream;
        _audioThread = new Thread(() =>
        {
            try
            {
                AudioLoop(pcm, cancellationToken);
                CompleteOnceSuccess();
            }
            catch (Exception ex) { CompleteOnceError(ex); }
        })
        {
            Name = "OpenAL-Audio"
        };
        _audioThread.Start();

        await _playbackTcs.Task;
        // 後片付け（明示）
        try
        {
            _audioThread.Join();
        }
        catch { }
        _running = false;
    }

    private void AudioLoop(Stream pcmS16, CancellationToken cancellationToken)
    {
        InitializeOpenAL();
        try
        {
            var format = (_opt.Channels, _bytesPerSample) switch
            {
                (1, 2) => ALFormat.Mono16,
                (2, 2) => ALFormat.Stereo16,
                _ => throw new NotSupportedException("このチャンネル数は未対応です。")
            };

            int frameBytes = _bytesPerSample * _opt.Channels;
            int bufferBytes = Math.Max(frameBytes, (_opt.SampleRate * _bufferMillis / 1000) * frameBytes);
            var work = new byte[bufferBytes];
            var eof = false;

            // 事前に数バッファをプライムしてから再生
            int primed = 0;
            for (int i = 0; i < _buffers.Length; i++)
            {
                var ok = FillAndQueue(_buffers[i], pcmS16, work, format, frameBytes);
                if (!ok)
                {
                    eof = true; // データがもう来ない
                    break;
                }
                primed++;
            }
            if (primed > 0)
            {
                AL.SourcePlay(_source);
                CheckALError();
            }

            while (_running && !cancellationToken.IsCancellationRequested)
            {
                AL.GetSource(_source, ALGetSourcei.BuffersProcessed, out int processed);
                while (processed-- > 0)
                {
                    int bid = AL.SourceUnqueueBuffer(_source);
                    CheckALError();


                    if (!eof)
                    {
                        var ok = FillAndQueue(bid, pcmS16, work, format, frameBytes);
                        if (!ok)
                        {
                            eof = true; // データがもう来ない
                        }
                    }

                }

                if (eof)
                {
                    AL.GetSource(_source, ALGetSourcei.BuffersQueued, out int queued);
                    if (queued == 0) break;
                }

                // アンダーランで止まっていたら再開
                AL.GetSource(_source, ALGetSourcei.SourceState, out int st);
                if ((ALSourceState)st != ALSourceState.Playing && (!_producerExited || !eof))
                {
                    AL.SourcePlay(_source);
                }
                Thread.Sleep(10); // 少し待機
            }
        }
        finally
        {
            ALC.MakeContextCurrent(ALContext.Null);
            _running = false;
        }
    }

    private bool FillAndQueue(int bufferId, Stream src, byte[] work, ALFormat fmt, int frameBytes)
    {
        int read = 0;
        while (read < work.Length)
        {
            int n = src.Read(work, read, work.Length - read);
            if (n <= 0) break;
            read += n;
            if (read >= frameBytes * 512) break; // 例: 512フレームで一旦キュー
        }
        if (read <= 0) return false; // データがもう来ない

        int size = read - (read % frameBytes);
        if (size <= 0) return true;

        // 端数は無音で埋める（クリック抑制）
        if (read < work.Length) Array.Clear(work, read, work.Length - read);

        AL.BufferData<byte>(bufferId, fmt, work.AsSpan(), _opt.SampleRate);
        AL.SourceQueueBuffer(_source, bufferId);
        CheckALError();
        return true;
    }

    public void Stop()
    {
        if (!_running) return;
        _running = false;

        try
        {
            AL.SourceStop(_source);
        }
        catch { /* ignore */ }

        // 残バッファ破棄
        AL.GetSource(_source, ALGetSourcei.BuffersQueued, out int queued);
        while (queued-- > 0)
        {
            _ = AL.SourceUnqueueBuffer(_source);
        }

        CompleteOnceSuccess();
    }

    public void Dispose()
    {
        try { Stop(); } catch { }
        try { if (_buffers.Length > 0) AL.DeleteBuffers(_buffers); } catch { }
        try { if (_source != 0) AL.DeleteSource(_source); } catch { }
        try
        {
            if (_context != ALContext.Null) { ALC.DestroyContext(_context); _context = ALContext.Null; }
            if (_device != ALDevice.Null) { ALC.CloseDevice(_device); _device = ALDevice.Null; }
        }
        catch { }
    }

    private static void CheckALError()
    {
        var err = AL.GetError();
        if (err != ALError.NoError) throw new InvalidOperationException($"OpenAL error: {err}");
    }
}