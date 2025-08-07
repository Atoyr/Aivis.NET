using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// HTTPバリデーションエラー
/// </summary>
/// <param name="Detail">エラー詳細の配列</param>
public record HttpValidationError(
    [property: JsonPropertyName("detail")] HttpValidationErrorDetail[]? Detail = null
);