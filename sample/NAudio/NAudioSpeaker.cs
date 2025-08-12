using NAudio.Wave;

using Aivis.Speakers;

namespace Aivis.Sample.NAudio;

public class NAudioSpeaker : ISpeaker
{

    public async Task PlayAsync(Stream audioStream, CancellationToken cancellationToken = default)
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