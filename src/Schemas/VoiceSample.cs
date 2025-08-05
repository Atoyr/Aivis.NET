using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record VoiceSample(
    [property: JsonPropertyName("audio_url")]
    string AudioUrl, 
    [property: JsonPropertyName("transcript")]
    string Transcript
);



