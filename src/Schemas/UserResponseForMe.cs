using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// 自分自身の情報を表すクラス。
/// </summary>
/// <param name="Handle">ユーザーのハンドル（識別子）。</param>
/// <param name="Name">ユーザーの名前。</param>
/// <param name="Description">ユーザーの説明文。</param>
/// <param name="IconUrl">ユーザーのアイコンURL。</param>
/// <param name="BirthYear">生まれた年</param>
/// <param name="Gender">性別</param>
/// <param name="Email">email</param>
/// <param name="AccountType">アカウントの種類。</param>
/// <param name="AccountStatus">アカウントのステータス。</param>
/// <param name="SocialLinks">ユーザーのソーシャルリンク一覧。</param>
/// <param name="CreditBalance">クレジット残高</param>
/// <param name="IsLowBalanceNotificationEnabled">クレジット残高の警告下限を下回ったときに通知するか</param>
/// <param name="LowBalanceThreshold">クレジット残高の警告下限</param>
/// <param name="LikedAivmModels">いいねをしたAIVMモデル</param>
/// <param name="AivmModels">自身のAIVMモデル</param>
/// <param name="CreatedAt">作成日時</param>
/// <param name="UpdatedAt">更新日時</param>
///
public record UserResponseForMe
(
    [property: JsonPropertyName("handle")]
    string Handle,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("icon_url")]
    string IconUrl,

    [property: JsonPropertyName("birth_year")]
    int? BirthYear, 

    [property: JsonPropertyName("gender")]
    Gender? Gender, 

    [property: JsonPropertyName("email")]
    string Email, 

    [property: JsonPropertyName("account_type")]
    AccountType AccountType,

    [property: JsonPropertyName("account_status")]
    AccountStatus AccountStatus,

    [property: JsonPropertyName("social_links")]
    SocialLink[] SocialLinks, 

    [property: JsonPropertyName("credit_balance")]
    int CreditBalance,

    [property: JsonPropertyName("is_low_balance_notification_enabled")]
    bool IsLowBalanceNotificationEnabled, 

    [property: JsonPropertyName("low_balance_threshold")]
    int LowBalanceThreshold, 

    [property: JsonPropertyName("liked_aivm_models")]
    AivmModelsResponse[] LikedAivmModels, 

    [property: JsonPropertyName("aivm_models")]
    AivmModelsResponse[] AivmModels, 

    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,

    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);
