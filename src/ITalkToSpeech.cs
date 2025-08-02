namespace Aivis;

public interface ITalkToSpeech
{
    /// <summary>
    /// 音声合成を行います。
    /// </summary>
    /// <param name="modelUuid">モデルのUUID</param>
    /// <param name="text">合成するテキスト</param>
    /// <param name="format">出力フォーマット（デフォルトは "mp3"）</param>
    /// <returns>音声データのバイト配列</returns>
    Task<byte[]> SynthesizeAsync(string modelUuid, string text, string format = "mp3");

    /// <summary>
    /// 音声合成を行いストリーミングします。
    /// </summary>
    /// <param name="modelUuid">モデルのUUID</param>
    /// <param name="text">合成するテキスト</param>
    /// <param name="format">出力フォーマット（デフォルトは "mp3"）</param>
    /// <returns>音声データのストリーム</returns>
    Task<Stream> SynthesizeStreamAsync(string modelUuid, string text, string format = "mp3");

    /// <summary>
    /// 音声合成を行います。
    /// クライアント側でクレジット残高の管理やレート制限の監視を行えます。
    /// </summary>
    /// <param name="modelUuid">モデルのUUID</param>
    /// <param name="text">合成するテキスト</param>
    /// <param name="format">出力フォーマット（デフォルトは "mp3"）</param>
    /// <returns>音声データとヘッダコンテンツを含むTTSコンテンツ</returns>
    Task<TTSContents> SynthesizeWithContentsAsync(string modelUuid, string text, string format = "mp3");
}