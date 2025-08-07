using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    [EnumMember(Value = nameof(Category.ExistingCharacter))]
    ExistingCharacter = 0,
    [EnumMember(Value = nameof(Category.OriginalCharacter))]
    OriginalCharacter = 1,
    [EnumMember(Value = nameof(Category.LivingPerson))]
    LivingPerson = 2,
    [EnumMember(Value = nameof(Category.DeceasedPerson))]
    DeceasedPerson = 3,
    [EnumMember(Value = nameof(Category.FictionalPerson))]
    FictionalPerson = 4,
    [EnumMember(Value = nameof(Category.Other))]
    Other = 5,
}