using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    [EnumMember(Value = "User")]
    User = 0,
    [EnumMember(Value = "Official")]    
    Official = 1,
    [EnumMember(Value = "Admin")]   
    Admin = 2
}