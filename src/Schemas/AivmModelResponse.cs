using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record AivmModelResponse
(
    [property: JsonPropertyName("aivm_model_uuid")]
    Guid AivmModelUuid, 

    [property: JsonPropertyName("user")]
    UserResponse User, 

    [property: JsonPropertyName("name")]
    string Name, 

    [property: JsonPropertyName("description")]
    string Description, 

    [property: JsonPropertyName("detailed_description")]
    string DetailedDescription, 

    [property: JsonPropertyName("category")]
    Category Category, 

    [property: JsonPropertyName("voice_timbre")]
    VoiceTimbre VoiceTimbre, 

    [property: JsonPropertyName("visibility")]
    Visibility Visibility, 

    [property: JsonPropertyName("is_tag_locked")]
    bool IsTagLocked, 

    [property: JsonPropertyName("model_files")]
    ModelFile[] ModelFiles, 

    [property: JsonPropertyName("tags")]
    Tag[] Tags, 

    [property: JsonPropertyName("like_count")]
    int LikeCount, 

    [property: JsonPropertyName("is_liked")]
    bool IsLiked, 

    [property: JsonPropertyName("speakers")]
    Speaker[] Speakers, 

    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt, 

    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);