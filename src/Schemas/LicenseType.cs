using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(EnumMemberStringEnumConverter<LicenseType>))]
public enum LicenseType
{
    [EnumMember(Value = "ACML 1.0")]
    ACML_1_0 = 0,
    [EnumMember(Value = "ACML-NC 1.0")]
    ACML_NC_1_0 = 1,
    [EnumMember(Value = "CC0")]
    CC0 = 2,
    [EnumMember(Value = "Custom")]
    Custom = 3,
    [EnumMember(Value = "Internal")]
    Internal = 4
}