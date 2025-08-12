namespace Aivis.Speakers;

public sealed class FFMpegDecodeOptions
{
    public string FFMpegPath { get; private set; } = "ffmpeg";

    public string LogLevel   { get; private set; } = "error"; // quiet/error/warning/info

    public int    SampleRate { get; private set; } = 48000;

    public int    Channels   { get; private set; } = 2;       // 1 or 2

    public string? InputFormat { get; init; } = "mp3"; // 入力がMP3等と分かっているなら指定

    public string  ExtraInputArgs  { get; private set; } = ""; // 例: "-re" など

    public string  ExtraOutputArgs { get; private set; } = ""; // 例: "-af aresample=async=1"

    public string BuildArgs()
    {
        var fmt = string.IsNullOrWhiteSpace(InputFormat) ? "" : $"-f {InputFormat} ";
        return $"-v {LogLevel} {fmt}{ExtraInputArgs} -i pipe:0 -vn " +
               $"-f s16le -ac {Channels} -ar {SampleRate} {ExtraOutputArgs} -acodec pcm_s16le -"; // PCM raw
    }

    public FFMpegDecodeOptions SetFFMpegPath(string path)
    {
        FFMpegPath = path;
        return this;
    }

    public FFMpegDecodeOptions SetLogLevel(string level)
    {
        LogLevel = level;
        return this;
    }

    public FFMpegDecodeOptions SetSampleRate(int rate)
    {
        SampleRate = rate;
        return this;
    }

    public FFMpegDecodeOptions SetChannels(int channels)
    {
        Channels = channels;
        return this;
    }

    public FFMpegDecodeOptions SetExtraInputArgs(string args)
    {
        ExtraInputArgs = args;
        return this;
    }

    public FFMpegDecodeOptions SetExtraOutputArgs(string args)
    {
        ExtraOutputArgs = args;
        return this;
    }
}