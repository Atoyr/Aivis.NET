using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// HTTPバリデーションエラーの詳細
/// </summary>
/// <param name="Loc">ロケーション</param>
/// <param name="Msg">メッセージ</param>
/// <param name="Type">タイプ</param>
public record HttpValidationErrorDetail
(
    [property: JsonPropertyName("loc")] object[] Loc,
    [property: JsonPropertyName("msg")] string Msg,
    [property: JsonPropertyName("type")] string Type
);