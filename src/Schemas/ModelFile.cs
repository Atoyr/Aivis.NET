using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public record ModelFile(
    [property: JsonPropertyName("aivm_model_uuid")]
    Guid AivmModelUuid,

    [property: JsonPropertyName("manifest_version")]
    string ManifestVersion,

    [property: JsonPropertyName("name")]
    string Name,

    [property: JsonPropertyName("description")]
    string Description,

    [property: JsonPropertyName("creators")]
    string[] Creators,

    [property: JsonPropertyName("license_type")]
    LicenseType LicenseType,

    [property: JsonPropertyName("license_text")]
    string? LicenseText,

    [property: JsonPropertyName("model_type")]
    ModelType ModelType,

    [property: JsonPropertyName("model_architecture")]
    ModelArchitecture ModelArchitecture,

    [property: JsonPropertyName("model_format")]
    ModelFormat ModelFormat,

    [property: JsonPropertyName("training_epoches")]
    int? TrainingEpochs,

    [property: JsonPropertyName("training_steps")]
    int? TrainingSteps,

    [property: JsonPropertyName("version")]
    string Version,

    [property: JsonPropertyName("file_size")]
    int FileSize,

    [property: JsonPropertyName("checksum")]
    string Checksum,

    [property: JsonPropertyName("download_count")]
    int DownloadCount,

    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,

    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);