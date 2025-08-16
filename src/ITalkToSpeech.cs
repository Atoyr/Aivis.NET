namespace Aivis;

/// <summary>
/// ITalkToSpeech インターフェースは、音声合成機能を提供します。
/// </summary>
public interface ITalkToSpeech
{
    /// <summary>
    /// 音声合成を行います。
    /// </summary>
    /// <param name="modelUuid">モデルのUUID</param>
    /// <param name="text">合成するテキスト</param>
    /// <param name="options">音声合成のオプション</param>
    /// <returns>音声データのバイト配列</returns>
    Task<byte[]> SynthesizeAsync(string modelUuid, string text, TalkToSpeechOptions? options = null);

    /// <summary>
    /// 音声合成を行いストリーミングします。
    /// </summary>
    /// <param name="modelUuid">モデルのUUID</param>
    /// <param name="text">合成するテキスト</param>
    /// <param name="options">音声合成のオプション</param>
    /// <returns>音声データのストリーム</returns>
    Task<Stream> SynthesizeStreamAsync(string modelUuid, string text, TalkToSpeechOptions? options = null);

    /// <summary>
    /// 音声合成を行います。
    /// クライアント側でクレジット残高の管理やレート制限の監視を行えます。
    /// </summary>
    /// <param name="modelUuid">モデルのUUID</param>
    /// <param name="text">合成するテキスト</param>
    /// <param name="options">音声合成のオプション</param>
    /// <returns>音声データとヘッダコンテンツを含むTTSコンテンツ</returns>
    Task<TTSContents> SynthesizeWithContentsAsync(string modelUuid, string text, TalkToSpeechOptions? options = null);
}