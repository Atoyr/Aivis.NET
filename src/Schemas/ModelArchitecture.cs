using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(EnumMemberStringEnumConverter<ModelArchitecture>))]
public enum ModelArchitecture
{
    [EnumMember(Value = "Style-Bert-VITS2")]
    [JsonPropertyName("Style-Bert-VITS2")]
    StyleBertVITS2 = 0,

    [EnumMember(Value = "Style-Bert-VITS2 (JP-Extra)")]
    [JsonPropertyName("Style-Bert-VITS2 (JP-Extra)")]
    StyleBertVITS2_JPEx = 1,
}