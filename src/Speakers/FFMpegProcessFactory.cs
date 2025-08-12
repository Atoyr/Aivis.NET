using System.Diagnostics;

namespace Aivis.Speakers;

/// <summary>
/// FFmpegプロセスを生成するファクトリクラス
/// </summary>
public static class FFmpegProcessFactory 
{
    public static Process CreateProcess(FFMpegDecodeOptions? options = null)
    {
        if (options == null)
        {
            options = new FFMpegDecodeOptions();
        }

        return new Process {
            StartInfo = new ProcessStartInfo {
                FileName = options.FFMpegPath,
                Arguments = options.BuildArgs(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
        };
    }
}