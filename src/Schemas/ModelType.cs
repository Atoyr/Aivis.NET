using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelType
{
    [EnumMember(Value = "AIVM")]
    AIVM = 0,
    [EnumMember(Value = "AIVMX")]
    AIVMX = 1
}