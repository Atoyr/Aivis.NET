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

    Task<TTSContents> SynthesizeWithContentsAsync(string modelUuid, string text, string format = "mp3");
}