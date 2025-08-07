using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// アカウント区分
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    /// <summary>
    /// ユーザー
    /// </summary>
    [EnumMember(Value = nameof(AccountType.User))]
    User = 0,
    /// <summary>
    /// 公式
    /// </summary>
    [EnumMember(Value = nameof(AccountType.Official))]
    Official = 1,
    /// <summary>
    /// 管理者
    /// </summary>
    [EnumMember(Value = nameof(AccountType.Admin))]
    Admin = 2
}