namespace Aivis.Speakers;

/// <summary>
/// ISpeaker インターフェースは、音声を再生する機能を提供します。
/// </summary>
public interface ISpeaker
{
    /// <summary>
    /// ストリームから音声を再生する
    /// </summary>
    /// <param name="audioStream">音声ストリーム</param>
    /// <param name="cancellationToken">キャンセル</param>
    Task PlayAsync(Stream audioStream, CancellationToken cancellationToken = default);
}