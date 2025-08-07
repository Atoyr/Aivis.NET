using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// モデルのフォーマットを表す列挙型。
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelFormat
{
    /// <summary>
    /// Safetensors形式のモデル。
    /// </summary>
    [EnumMember(Value = "Safetensors")]
    Safetensors = 0,

    /// <summary>
    /// ONNX形式のモデル。
    /// </summary>
    [EnumMember(Value = "ONNX")]
    Onnx = 1,
}