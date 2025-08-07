using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// スタイル情報を表すクラス。
/// </summary>
/// <param name="Name">スタイル名。</param>
/// <param name="IconUrl">スタイルのアイコンURL。</param>
/// <param name="LocalId">ローカルID。</param>
/// <param name="VoiceSamples">スタイルの音声サンプル一覧。</param>
public record Style(
    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("icon_url")]
    string? IconUrl,

    [property: JsonPropertyName("local_id")]
    int LocalId,

    [property: JsonPropertyName("voice_samples")]
    VoiceSample[] VoiceSamples
);