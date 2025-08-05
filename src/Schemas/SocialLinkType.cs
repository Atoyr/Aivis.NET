using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SocialLinkType
{
    [EnumMember(Value = nameof(SocialLinkType.Twitter))]
    Twitter = 0,
    [EnumMember(Value = nameof(SocialLinkType.Bluesky))]
    Bluesky = 1,
    [EnumMember(Value = nameof(SocialLinkType.Misskey))]
    Misskey = 2,
    [EnumMember(Value = nameof(SocialLinkType.YouTube))]
    YouTube = 3,
    [EnumMember(Value = nameof(SocialLinkType.Niconico))]
    Niconico = 4,
    [EnumMember(Value = nameof(SocialLinkType.Instagram))]
    Instagram = 5,
    [EnumMember(Value = nameof(SocialLinkType.TikTok))]
    TikTok = 6,
    [EnumMember(Value = nameof(SocialLinkType.Facebook))]
    Facebook = 7,
    [EnumMember(Value = nameof(SocialLinkType.GitHub))]
    GitHub = 8,
    [EnumMember(Value = nameof(SocialLinkType.HuggingFace))]
    HuggingFace = 8,
    [EnumMember(Value = nameof(SocialLinkType.Website))]
    Website = 10
}