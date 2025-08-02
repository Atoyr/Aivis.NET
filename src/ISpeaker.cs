namespace Aivis;

public interface ISpeaker
{
    /// <summary>
    /// バイナリデータから音声を再生する
    /// </summary>
    /// <param name="mediaType">音声バイナリデータのコンテンツタイプ</param>
    /// <param name="audioData">音声バイナリデータ</param>
    /// <param name="cancellationToken">キャンセル</param>
    Task PlayAsync(MediaType mediaType, byte[] audioData, CancellationToken cancellationToken);

    /// <summary>
    /// ストリームから音声を再生する
    /// </summary>
    /// <param name="mediaType">音声バイナリデータのコンテンツタイプ</param>
    /// <param name="audioStream">音声ストリーム</param>
    /// <param name="cancellationToken">キャンセル</param>
    Task PlayAsync(MediaType mediaType, Stream audioStream, CancellationToken cancellationToken);

    /// <summary>
    /// バイナリデータから音声を再生する
    /// </summary>
    /// <param name="mediaType">音声バイナリデータのコンテンツタイプ</param>
    /// <param name="audioData">音声バイナリデータ</param>
    /// <param name="cancellationToken">キャンセル</param>
    void Play(MediaType meidaType, byte[] audioData);
}