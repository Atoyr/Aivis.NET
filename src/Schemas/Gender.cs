using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// 性別
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    /// <summary>
    /// 男性
    /// </summary>
    [EnumMember(Value = "Male")]
    Male = 0,

    /// <summary>
    /// 女性
    /// </summary>
    [EnumMember(Value = "Female")]
    Female = 1,

    /// <summary>
    /// その他
    /// </summary>
    [EnumMember(Value = "Other")]
    Other = 2,
}