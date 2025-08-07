using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Visibility
{
    /// <summary>
    /// 公開
    /// </summary>
    [EnumMember(Value = nameof(Visibility.Public))]
    Public = 0,

    /// <summary>
    /// 非公開
    /// </summary>
    [EnumMember(Value = nameof(Visibility.Private))]
    Private = 1,

    /// <summary>
    /// 管理者のみ閲覧可能
    /// </summary>
    [EnumMember(Value = nameof(Visibility.AdminHidden))]
    AdminHidden = 2
}