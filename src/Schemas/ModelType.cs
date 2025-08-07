using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelType
{
    [EnumMember(Value = "AIVM")]
    AIVM = 0,
    [EnumMember(Value = "AIVMX")]
    AIVMX = 1
}