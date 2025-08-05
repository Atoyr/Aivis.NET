using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record SocialLink(
    [property: JsonPropertyName("type")]
    SocialLinkType Type, 
    [property: JsonPropertyName("url")]
    string Url
);