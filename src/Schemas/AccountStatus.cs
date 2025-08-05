using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountStatus
{
    /// <summary>
    /// アクティブ
    /// </summary>
    [EnumMember(Value = nameof(AccountStatus.Active))]
    Active = 0,

    /// <summary>
    /// 保留
    /// </summary>
    [EnumMember(Value = nameof(AccountStatus.Suspended))]
    Suspended = 1,
}