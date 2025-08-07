using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// モデルの種類を表す列挙型。
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelType
{
    /// <summary>
    /// AIVM形式のモデル。
    /// </summary>
    [EnumMember(Value = "AIVM")]
    AIVM = 0,

    /// <summary>
    /// AIVMX形式のモデル。
    /// </summary>
    [EnumMember(Value = "AIVMX")]
    AIVMX = 1
}