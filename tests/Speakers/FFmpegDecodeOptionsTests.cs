using Aivis.Speakers;

namespace Aivis.Tests.Speakers;

public class FFmpegDecodeOptionsTests
{
    [Fact]
    public void FFmpegDecodeOptions_DefaultValues_AreSetCorrectly()
    {
        var options = new FFmpegDecodeOptions();

        Assert.Equal("ffmpeg", options.FFmpegPath);
        Assert.Equal("error", options.LogLevel);
        Assert.Equal(48000, options.SampleRate);
        Assert.Equal(2, options.Channels);
        Assert.Equal("mp3", options.InputFormat);
        Assert.Equal("", options.ExtraInputArgs);
        Assert.Equal("", options.ExtraOutputArgs);
    }

    [Fact]
    public void SetFFmpegPath_SetsCorrectPath()
    {
        var options = new FFmpegDecodeOptions();
        options.SetFFmpegPath("/usr/bin/ffmpeg");

        Assert.Equal("/usr/bin/ffmpeg", options.FFmpegPath);
    }

    [Fact]
    public void SetLogLevel_SetsCorrectLogLevel()
    {
        var options = new FFmpegDecodeOptions();
        options.SetLogLevel("debug");

        Assert.Equal("debug", options.LogLevel);
    }

    [Fact]
    public void SetSampleRate_SetsCorrectSampleRate()
    {
        var options = new FFmpegDecodeOptions();
        options.SetSampleRate(44100);

        Assert.Equal(44100, options.SampleRate);
    }

    [Fact]
    public void SetChannels_SetsCorrectChannels()
    {
        var options = new FFmpegDecodeOptions();
        options.SetChannels(1);

        Assert.Equal(1, options.Channels);
    }

    [Fact]
    public void SetExtraInputArgs_SetsCorrectArgs()
    {
        var options = new FFmpegDecodeOptions();
        options.SetExtraInputArgs("-re");

        Assert.Equal("-re", options.ExtraInputArgs);
    }

    [Fact]
    public void SetExtraOutputArgs_SetsCorrectArgs()
    {
        var options = new FFmpegDecodeOptions();
        options.SetExtraOutputArgs("-af aresample=async=1");

        Assert.Equal("-af aresample=async=1", options.ExtraOutputArgs);
    }

    [Fact]
    public void BuildArgs_WithDefaults_ReturnsCorrectArgs()
    {
        var options = new FFmpegDecodeOptions();
        var args = options.BuildArgs();

        Assert.Equal("-v error -f mp3  -i pipe:0 -vn -f s16le -ac 2 -ar 48000  -acodec pcm_s16le -", args);
    }

    [Fact]
    public void BuildArgs_WithCustomValues_ReturnsCorrectArgs()
    {
        var options = new FFmpegDecodeOptions();
        options.SetLogLevel("debug")
               .SetChannels(1)
               .SetSampleRate(44100)
               .SetExtraInputArgs("-re")
               .SetExtraOutputArgs("-af volume=0.5");

        var args = options.BuildArgs();

        Assert.Equal("-v debug -f mp3 -re -i pipe:0 -vn -f s16le -ac 1 -ar 44100 -af volume=0.5 -acodec pcm_s16le -", args);
    }

    [Fact]
    public void BuildArgs_WithNullInputFormat_DoesNotIncludeFormat()
    {
        var options = new FFmpegDecodeOptions { InputFormat = null };
        var args = options.BuildArgs();

        Assert.Equal("-v error  -i pipe:0 -vn -f s16le -ac 2 -ar 48000  -acodec pcm_s16le -", args);
    }

    [Fact]
    public void BuildArgs_WithEmptyInputFormat_DoesNotIncludeFormat()
    {
        var options = new FFmpegDecodeOptions { InputFormat = "" };
        var args = options.BuildArgs();

        Assert.Equal("-v error  -i pipe:0 -vn -f s16le -ac 2 -ar 48000  -acodec pcm_s16le -", args);
    }
}