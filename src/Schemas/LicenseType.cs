using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LicenseType
{
    [EnumMember(Value = "ACML 1.0")]
    [JsonPropertyName("ACML 1.0")]
    ACML_1_0 = 0,
    [EnumMember(Value = "ACML-NC 1.0")]
    [JsonPropertyName("ACML-NC 1.0")]
    ACML_NC_1_0 = 1,
    [EnumMember(Value = "CC0")]
    [JsonPropertyName("CC0")]
    CC0 = 2,
    [EnumMember(Value = "Custom")]
    [JsonPropertyName("Custom")]
    Custom = 3,
    [EnumMember(Value = "Internal")]
    [JsonPropertyName("Internal")]
    Internal = 4
}
