using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record HttpValidationErrorItem
(
    [property: JsonPropertyName("loc")] object[] Loc,
    [property: JsonPropertyName("msg")] string Msg,
    [property: JsonPropertyName("type")] string Type
);