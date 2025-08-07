using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// ライセンス種別
/// </summary>
[JsonConverter(typeof(EnumMemberStringEnumConverter<LicenseType>))]
public enum LicenseType
{
    /// <summary>
    /// ACML 1.0
    /// </summary>
    [EnumMember(Value = "ACML 1.0")]
    ACML_1_0 = 0,

    /// <summary>
    /// ACML-NC 1.0
    /// </summary>
    [EnumMember(Value = "ACML-NC 1.0")]
    ACML_NC_1_0 = 1,

    /// <summary>
    /// CC0
    /// </summary>
    [EnumMember(Value = "CC0")]
    CC0 = 2,

    /// <summary>
    /// カスタム
    /// </summary>
    [EnumMember(Value = "Custom")]
    Custom = 3,

    /// <summary>
    /// 内部
    /// </summary>
    [EnumMember(Value = "Internal")]
    Internal = 4
}