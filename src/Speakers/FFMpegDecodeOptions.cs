namespace Aivis.Speakers;

/// <summary>
/// FFmpeg を使用してデコードするためのオプションを設定するクラス。
/// </summary>
public sealed class FFmpegDecodeOptions
{
    /// <summary>
    /// FFmpeg の実行パス。
    /// </summary>
    public string FFmpegPath { get; private set; } = "ffmpeg";

    /// <summary>
    /// ログレベルを指定します。 (quiet/error/warning/info)
    /// </summary>
    public string LogLevel { get; private set; } = "error"; // quiet/error/warning/info

    /// <summary>
    /// サンプルレートを指定します。
    /// </summary>
    public int SampleRate { get; private set; } = 48000;

    /// <summary>
    /// チャンネル数を指定します。 (1 または 2)
    /// </summary>
    public int Channels { get; private set; } = 2;       // 1 or 2

    /// <summary>
    /// 入力フォーマットを指定します (例: "mp3")。
    /// 入力が MP3 などと分かっている場合に指定します。
    /// </summary>
    public string? InputFormat { get; init; } = "mp3"; // 入力がMP3等と分かっているなら指定

    /// <summary>
    /// 追加の入力引数を指定します (例: "-re" など)。
    /// </summary>
    public string ExtraInputArgs { get; private set; } = ""; // 例: "-re" など

    /// <summary>
    /// 追加の出力引数を指定します (例: "-af aresample=async=1")。
    /// </summary>
    public string ExtraOutputArgs { get; private set; } = ""; // 例: "-af aresample=async=1"

    /// <summary>
    /// FFmpeg のコマンドライン引数を構築します。
    /// </summary>
    /// <returns>構築された引数の文字列。</returns>
    public string BuildArgs()
    {
        var fmt = string.IsNullOrWhiteSpace(InputFormat) ? "" : $"-f {InputFormat} ";
        return $"-v {LogLevel} {fmt}{ExtraInputArgs} -i pipe:0 -vn " +
               $"-f s16le -ac {Channels} -ar {SampleRate} {ExtraOutputArgs} -acodec pcm_s16le -"; // PCM raw
    }

    /// <summary>
    /// FFmpeg の実行パスを設定します。
    /// </summary>
    /// <param name="path">FFmpeg の実行パス。</param>
    /// <returns>現在のインスタンス。</returns>
    public FFmpegDecodeOptions SetFFmpegPath(string path)
    {
        FFmpegPath = path;
        return this;
    }

    /// <summary>
    /// ログレベルを設定します。
    /// </summary>
    /// <param name="level">ログレベル。</param>
    /// <returns>現在のインスタンス。</returns>
    public FFmpegDecodeOptions SetLogLevel(string level)
    {
        LogLevel = level;
        return this;
    }

    /// <summary>
    /// サンプルレートを設定します。
    /// </summary>
    /// <param name="rate">サンプルレート (正の値)。</param>
    /// <returns>現在のインスタンス。</returns>
    public FFmpegDecodeOptions SetSampleRate(int rate)
    {
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), "SampleRate must be positive.");
        SampleRate = rate;
        return this;
    }

    /// <summary>
    /// チャンネル数を設定します。
    /// </summary>
    /// <param name="channels">チャンネル数 (1 または 2)。</param>
    /// <returns>現在のインスタンス。</returns>
    public FFmpegDecodeOptions SetChannels(int channels)
    {
        if (channels is not 1 and not 2) throw new ArgumentOutOfRangeException(nameof(channels), "Channels must be 1 (mono) or 2 (stereo).");
        Channels = channels;
        return this;
    }

    /// <summary>
    /// 追加の入力引数を設定します。
    /// </summary>
    /// <param name="args">追加の入力引数。</param>
    /// <returns>現在のインスタンス。</returns>
    public FFmpegDecodeOptions SetExtraInputArgs(string args)
    {
        ExtraInputArgs = args;
        return this;
    }

    /// <summary>
    /// 追加の出力引数を設定します。
    /// </summary>
    /// <param name="args">追加の出力引数。</param>
    /// <returns>現在のインスタンス。</returns>
    public FFmpegDecodeOptions SetExtraOutputArgs(string args)
    {
        ExtraOutputArgs = args;
        return this;
    }
}