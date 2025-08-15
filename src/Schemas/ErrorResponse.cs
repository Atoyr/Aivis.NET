using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// エラーの汎用レスポンス
/// </summary>
/// <param name="Detail">エラー内容</param>
public record ErrorResponse(
    [property: JsonPropertyName("detail")] string? Detail = null
);