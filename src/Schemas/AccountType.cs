using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    [EnumMember(Value = nameof(AccountType.User))]
    User = 0,
    [EnumMember(Value = nameof(AccountType.Official))]
    Official = 1,
    [EnumMember(Value = nameof(AccountType.Admin))]
    Admin = 2
}