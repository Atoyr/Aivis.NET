using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// HTTPバリデーションエラー
/// </summary>
public record HttpValidationError(
    [property: JsonPropertyName("detail")] HttpValidationErrorItem[]? Detail = null
);