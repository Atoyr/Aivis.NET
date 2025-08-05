using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// 音声の声質を表す列挙型です。
/// Json デシリアライズ時に文字列から適切な列挙値に変換されます。
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VoiceTimbre
{
    /// <summary>若い男性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.YoungMale))]
    YoungMale,

    /// <summary>若い女性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.YoungFemale))]
    YoungFemale,

    /// <summary>少年の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.YouthfulMale))]
    YouthfulMale,

    /// <summary>少女の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.YouthfulFemale))]
    YouthfulFemale,

    /// <summary>成人男性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.AdultMale))]
    AdultMale,

    /// <summary>成人女性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.AdultFemale))]
    AdultFemale,

    /// <summary>中年男性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.MiddleAgedMale))]
    MiddleAgedMale,

    /// <summary>中年女性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.MiddleAgedFemale))]
    MiddleAgedFemale,

    /// <summary>高齢男性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.ElderlyMale))]
    ElderlyMale,

    /// <summary>高齢女性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.ElderlyFemale))]
    ElderlyFemale,

    /// <summary>中性の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.Neutral))]
    Neutral,

    /// <summary>赤ちゃんの声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.Baby))]
    Baby,

    /// <summary>機械的な声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.Mechanical))]
    Mechanical,

    /// <summary>その他の声</summary>
    [EnumMember(Value = nameof(VoiceTimbre.Other))]
    Other
}