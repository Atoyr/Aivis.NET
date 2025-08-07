using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// 音声合成のための話者情報を表すクラス。
/// </summary>
/// <param name="AivmSpeakerUuid">話者のUUID。</param>
/// <param name="Name">話者の名前。</param>
/// <param name="IconUrl">話者のアイコンURL。</param>
/// <param name="SupportedLanguages">話者がサポートする言語。</param>
/// <param name="LocalId">ローカルID。</param>
/// <param name="Styles">話者のスタイル一覧。</param>
public record Speaker(
    [property: JsonPropertyName("aivm_speaker_uuid")]
    Guid AivmSpeakerUuid,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("icon_url")]
    string IconUrl,

    [property: JsonPropertyName("supported_languages")]
    string[] SupportedLanguages,

    [property: JsonPropertyName("local_id")]
    int LocalId,

    [property: JsonPropertyName("styles")]
    Style[] Styles
);