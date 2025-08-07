using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// モデル アーキテクチャ
/// </summary>
[JsonConverter(typeof(EnumMemberStringEnumConverter<ModelArchitecture>))]
public enum ModelArchitecture
{
    /// <summary>
    /// Style-Bert-VITS2
    /// </summary>
    [EnumMember(Value = "Style-Bert-VITS2")]
    StyleBertVITS2 = 0,

    /// <summary>
    /// Style-Bert-VITS2 (JP-Extra)
    /// </summary>
    [EnumMember(Value = "Style-Bert-VITS2 (JP-Extra)")]
    StyleBertVITS2_JPEx = 1,
}