using NAudio.Wave;

namespace Aivis;

public class Speaker : ISpeaker
{
    public void Play(MediaType mediaType, byte[] audioData)
    {
        PlayAsync(mediaType, audioData).GetAwaiter().GetResult();
    }

    public async Task PlayAsync(MediaType mediaType, byte[] audioData, CancellationToken cancellationToken = default)
    {
        // ここに非同期で音声を再生するロジックを実装
        switch (mediaType)
        {
            case MediaType.MP3:
                await PlayMp3Async(audioData, cancellationToken);
                break;
            default:
                throw new NotSupportedException($"Unsupported media type: {mediaType}");
        }
    }

    private async Task PlayMp3Async(byte[] audioData, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(audioData);
        stream.Position = 0;

        var mp3Reader = new Mp3FileReader(stream);
        // 出力デバイスを初期化
        var outputDevice = new WaveOutEvent();
        outputDevice.Init(mp3Reader);
        
        // 再生開始
        outputDevice.Play();

        while (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                outputDevice.Stop();
                return;
            }
            await Task.Delay(100);
        }
    }
}