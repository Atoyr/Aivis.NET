using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record SocialLink(
    [property: JsonPropertyName("type")]
    string Type, 
    [property: JsonPropertyName("url")]
    string Url
);