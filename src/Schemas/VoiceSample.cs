using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// ボイスのサンプルを表すレコード型。
/// </summary>
/// <param name="AudioUrl">音声のURL</param>
/// <param name="Transcript">変換するメッセージ</param>
public record VoiceSample(
    [property: JsonPropertyName("audio_url")]
    string AudioUrl,
    [property: JsonPropertyName("transcript")]
    string Transcript
);