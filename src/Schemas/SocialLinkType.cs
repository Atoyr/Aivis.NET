using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// ソーシャルリンクの種類を表す列挙型。
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SocialLinkType
{
    /// <summary>
    /// Twitterのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Twitter))]
    Twitter = 0,

    /// <summary>
    /// Blueskyのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Bluesky))]
    Bluesky = 1,

    /// <summary>
    /// Misskeyのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Misskey))]
    Misskey = 2,

    /// <summary>
    /// YouTubeのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.YouTube))]
    YouTube = 3,

    /// <summary>
    /// Niconicoのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Niconico))]
    Niconico = 4,

    /// <summary>
    /// Instagramのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Instagram))]
    Instagram = 5,

    /// <summary>
    /// TikTokのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.TikTok))]
    TikTok = 6,

    /// <summary>
    /// Facebookのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Facebook))]
    Facebook = 7,

    /// <summary>
    /// GitHubのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.GitHub))]
    GitHub = 8,

    /// <summary>
    /// HuggingFaceのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.HuggingFace))]
    HuggingFace = 8,

    /// <summary>
    /// ウェブサイトのリンク。
    /// </summary>
    [EnumMember(Value = nameof(SocialLinkType.Website))]
    Website = 10
}