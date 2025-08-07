using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelFormat
{
    [EnumMember(Value = "Safetensors")]
    [JsonPropertyName("Safetensors")]
    Safetensors = 0,
    [EnumMember(Value = "ONNX")]
    [JsonPropertyName("ONNX")]
    Onnx = 1,
}