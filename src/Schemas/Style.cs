using System.Text.Json.Serialization;

namespace Aivis.Schemas;

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