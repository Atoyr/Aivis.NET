using System.Diagnostics;

namespace Aivis.Speakers;

/// <summary>
/// FFmpegプロセスを生成するファクトリクラス
/// </summary>
public static class FFmpegProcessFactory
{
    /// <summary>
    /// FFmpegDecodeOptionsを使用してFFmpegプロセスを生成します。
    /// </summary>
    /// <param name="options">FFmpegDecodeOptionsのインスタンス。nullの場合はデフォルトオプションが使用されます。</param>
    /// <returns>生成されたFFmpegプロセスのインスタンス。</returns>
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