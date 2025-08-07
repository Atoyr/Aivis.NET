using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// カテゴリー
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    /// <summary>
    /// 既存キャラクター
    /// </summary>
    [EnumMember(Value = nameof(Category.ExistingCharacter))]
    ExistingCharacter = 0,

    /// <summary>
    /// オリジナルキャラクター
    /// </summary>
    [EnumMember(Value = nameof(Category.OriginalCharacter))]
    OriginalCharacter = 1,

    /// <summary>
    /// 実在する人
    /// </summary>
    [EnumMember(Value = nameof(Category.LivingPerson))]
    LivingPerson = 2,

    /// <summary>
    /// 故人
    /// </summary>
    [EnumMember(Value = nameof(Category.DeceasedPerson))]
    DeceasedPerson = 3,

    /// <summary>
    /// 架空の人
    /// </summary>
    [EnumMember(Value = nameof(Category.FictionalPerson))]
    FictionalPerson = 4,

    /// <summary>
    /// その他
    /// </summary>
    [EnumMember(Value = nameof(Category.Other))]
    Other = 5,
}