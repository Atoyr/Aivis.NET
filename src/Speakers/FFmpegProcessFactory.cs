using System.Diagnostics;

namespace Aivis.Speakers;

/// <summary>
/// FFmpegプロセスを生成するファクトリクラス
/// </summary>
public static class FFmpegProcessFactory
{
    public static Process CreateProcess(FFmpegDecodeOptions? options = null)
    {
        if (options == null)
        {
            options = new FFmpegDecodeOptions();
        }

        return new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = options.FFmpegPath,
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