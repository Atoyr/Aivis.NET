using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// タグ
/// </summary>
/// <param name="Name">タグ名</param>
public record Tag(
    [property: JsonPropertyName("name")]
    string Name
);