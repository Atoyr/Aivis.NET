using NAudio.Wave;

namespace Aivis;

public class Speaker : ISpeaker
{
    public void Play(MediaType mediaType, byte[] audioData)
    {
        PlayAsync(mediaType, audioData).GetAwaiter().GetResult();
    }

    public async Task PlayAsync(MediaType mediaType, Stream audioStream, CancellationToken cancellationToken = default)
    {
        // ここに非同期で音声を再生するロジックを実装
        switch (mediaType)
        {
            case MediaType.MP3:
                await PlayMp3Async(audioStream, cancellationToken);
                break;
            default:
                throw new NotSupportedException($"Unsupported media type: {mediaType}");
        }
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
        await PlayMp3Async(stream, cancellationToken);
    }

    private async Task PlayMp3Async(Stream audioStream, CancellationToken cancellationToken = default)
    {
        if (audioStream.CanSeek)
        {
            audioStream.Position = 0;
        }

        using var mp3Reader = new Mp3FileReader(audioStream);
        using var outputDevice = new WaveOutEvent();
        // 出力デバイスを初期化
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
            await Task.Delay(100, cancellationToken);
        }
    }
}