using System.Diagnostics;

using Aivis.Speakers;

namespace Aivis.Tests.Speakers;

public class FFMpegProcessFactoryTests
{
    [Fact]
    public void CreateProcess_WithDefaultOptions_CreatesValidProcess()
    {
        var process = FFmpegProcessFactory.CreateProcess();

        Assert.NotNull(process);
        Assert.Equal("ffmpeg", process.StartInfo.FileName);
        Assert.True(process.StartInfo.RedirectStandardInput);
        Assert.True(process.StartInfo.RedirectStandardOutput);
        Assert.True(process.StartInfo.RedirectStandardError);
        Assert.False(process.StartInfo.UseShellExecute);
        Assert.True(process.StartInfo.CreateNoWindow);
    }

    [Fact]
    public void CreateProcess_WithCustomOptions_CreatesProcessWithCustomArgs()
    {
        var options = new FFMpegDecodeOptions();
        options.SetFFMpegPath("/custom/ffmpeg")
               .SetLogLevel("debug")
               .SetSampleRate(44100)
               .SetChannels(1);

        var process = FFmpegProcessFactory.CreateProcess(options);

        Assert.NotNull(process);
        Assert.Equal("/custom/ffmpeg", process.StartInfo.FileName);
        Assert.Contains("debug", process.StartInfo.Arguments);
        Assert.Contains("44100", process.StartInfo.Arguments);
        Assert.Contains("-ac 1", process.StartInfo.Arguments);
    }

    [Fact]
    public void CreateProcess_WithNullOptions_UsesDefaults()
    {
        var process = FFmpegProcessFactory.CreateProcess(null);

        Assert.NotNull(process);
        Assert.Equal("ffmpeg", process.StartInfo.FileName);
        Assert.Contains("-v error", process.StartInfo.Arguments);
        Assert.Contains("-ar 48000", process.StartInfo.Arguments);
        Assert.Contains("-ac 2", process.StartInfo.Arguments);
    }
}