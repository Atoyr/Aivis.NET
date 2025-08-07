using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record Tag(
    [property: JsonPropertyName("name")]
    string Name
);