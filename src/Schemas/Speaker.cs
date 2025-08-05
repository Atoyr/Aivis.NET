using System.Text.Json.Serialization;

namespace Aivis.Schemas;

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

