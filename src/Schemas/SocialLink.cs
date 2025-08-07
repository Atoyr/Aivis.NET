using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// ソーシャルリンクを表すレコード。
/// </summary>
/// <param name="Type">ソーシャルリンクの種類。</param>
/// <param name="Url">ソーシャルリンクのURL。</param>
public record SocialLink(
    [property: JsonPropertyName("type")]
    SocialLinkType Type,

    [property: JsonPropertyName("url")]
    string Url
);