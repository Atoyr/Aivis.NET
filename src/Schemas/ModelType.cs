using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelType
{
    [EnumMember(Value = "AIVM")]
    Aivm = 0,
    [EnumMember(Value = "AIVMX")]
    Aivmx = 1
}
