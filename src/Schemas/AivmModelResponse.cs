using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// AIVMモデルのレスポンスモデル
/// </summary>
public record AivmModelResponse
(
    /// <summary>
    /// AIVMモデルUUID
    /// </summary>
    [property: JsonPropertyName("aivm_model_uuid")]
    Guid AivmModelUuid,

    /// <summary>
    /// ユーザー情報
    /// 公開可能な情報のみを含む (メールアドレスなどの個人情報は含まない)
    /// </summary>
    [property: JsonPropertyName("user")]
    UserResponse User,

    /// <summary>
    /// 名前
    /// </summary>
    [property: JsonPropertyName("name")]
    string Name,

    /// <summary>
    /// 説明
    /// </summary>
    [property: JsonPropertyName("description")]
    string Description,

    /// <summary>
    /// 詳細説明
    /// </summary>
    [property: JsonPropertyName("detailed_description")]
    string DetailedDescription,

    /// <summary>
    /// カテゴリー
    /// </summary>
    [property: JsonPropertyName("category")]
    Category Category,

    /// <summary>
    /// 声質
    /// </summary>
    [property: JsonPropertyName("voice_timbre")]
    VoiceTimbre VoiceTimbre,

    /// <summary>
    /// 公開範囲
    /// </summary>
    [property: JsonPropertyName("visibility")]
    Visibility Visibility,

    /// <summary>
    /// タグロックしている場合True
    /// </summary>
    [property: JsonPropertyName("is_tag_locked")]
    bool IsTagLocked,

    /// <summary>
    /// モデルファイルの配列
    /// </summary>
    [property: JsonPropertyName("model_files")]
    ModelFile[] ModelFiles,

    /// <summary>
    /// タグ一覧
    /// </summary>
    [property: JsonPropertyName("tags")]
    Tag[] Tags,

    /// <summary>
    /// お気に入り数
    /// </summary>
    [property: JsonPropertyName("like_count")]
    int LikeCount,

    /// <summary>
    /// 認証しているユーザーがお気に入りした場合True
    /// 認証していない状態で取得した場合False
    /// </summary>
    [property: JsonPropertyName("is_liked")]
    bool IsLiked,

    /// <summary>
    /// 話者の配列
    /// </summary>
    [property: JsonPropertyName("speakers")]
    Speaker[] Speakers,

    /// <summary>
    /// 作成日
    /// </summary>
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,

    /// <summary>
    /// 更新日
    /// </summary>
    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);