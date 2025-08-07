using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// ユーザー情報を表すクラス。
/// </summary>
/// <param name="Handle">ユーザーのハンドル（識別子）。</param>
/// <param name="Name">ユーザーの名前。</param>
/// <param name="Description">ユーザーの説明文。</param>
/// <param name="IconUrl">ユーザーのアイコンURL。</param>
/// <param name="AccountType">アカウントの種類。</param>
/// <param name="AccountStatus">アカウントのステータス。</param>
/// <param name="SocialLinks">ユーザーのソーシャルリンク一覧。</param>
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