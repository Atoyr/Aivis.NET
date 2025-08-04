using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record AivmModelsResponse
(
    [property: JsonPropertyName("total")]
    int Total, 
    [property: JsonPropertyName("aivm_models")]
    IEnumerable<AivmModelResponse> AivmModels
);