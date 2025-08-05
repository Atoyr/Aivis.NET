using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record UserResponse
(
    [property: JsonPropertyName("handle")]
    string Handle,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("icon_url")]
    string IconUrl,

    [property: JsonPropertyName("account_type")]
    AccountType AccountType,

    [property: JsonPropertyName("account_status")]
    AccountStatus AccountStatus,

    [property: JsonPropertyName("social_links")]
    SocialLink[] SocialLinks
);