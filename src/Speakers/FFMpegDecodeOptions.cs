namespace Aivis.Speakers;

public sealed class FFmpegDecodeOptions
{
    public string FFmpegPath { get; private set; } = "ffmpeg";

    public string LogLevel { get; private set; } = "error"; // quiet/error/warning/info

    public int SampleRate { get; private set; } = 48000;

    public int Channels { get; private set; } = 2;       // 1 or 2

    public string? InputFormat { get; init; } = "mp3"; // 入力がMP3等と分かっているなら指定

    public string ExtraInputArgs { get; private set; } = ""; // 例: "-re" など

    public string ExtraOutputArgs { get; private set; } = ""; // 例: "-af aresample=async=1"

    public string BuildArgs()
    {
        var fmt = string.IsNullOrWhiteSpace(InputFormat) ? "" : $"-f {InputFormat} ";
        return $"-v {LogLevel} {fmt}{ExtraInputArgs} -i pipe:0 -vn " +
               $"-f s16le -ac {Channels} -ar {SampleRate} {ExtraOutputArgs} -acodec pcm_s16le -"; // PCM raw
    }

    public FFmpegDecodeOptions SetFFmpegPath(string path)
    {
        FFmpegPath = path;
        return this;
    }

    public FFmpegDecodeOptions SetLogLevel(string level)
    {
        LogLevel = level;
        return this;
    }

    public FFmpegDecodeOptions SetSampleRate(int rate)
    {
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate), "SampleRate must be positive.");
        SampleRate = rate;
        return this;
    }

    public FFmpegDecodeOptions SetChannels(int channels)
    {
        if (channels is not 1 and not 2) throw new ArgumentOutOfRangeException(nameof(channels), "Channels must be 1 (mono) or 2 (stereo).");
        Channels = channels;
        return this;
    }

    public FFmpegDecodeOptions SetExtraInputArgs(string args)
    {
        ExtraInputArgs = args;
        return this;
    }

    public FFmpegDecodeOptions SetExtraOutputArgs(string args)
    {
        ExtraOutputArgs = args;
        return this;
    }
}