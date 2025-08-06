using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class ModelFileTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "aivm_model_uuid": "a59cb814-0083-4369-8542-f51a29e72af7",
            "manifest_version": "1.0",
            "name": "Test Model",
            "description": "Test Description",
            "creators": ["Creator1", "Creator2"],
            "license_type": "ACML 1.0",
            "license_text": null,
            "model_type": "AIVM",
            "model_architecture": "Style-Bert-VITS2",
            "model_format": "Safetensors",
            "training_epoches": 116,
            "training_steps": 32000,
            "version": "1.0.1",
            "file_size": 257639874,
            "checksum": "51a69c4218b73218",
            "download_count": 186,
            "created_at": "2025-04-06T07:59:47.064137+09:00",
            "updated_at": "2025-08-01T19:10:51.099067+09:00"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<ModelFile>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("a59cb814-0083-4369-8542-f51a29e72af7"), result.AivmModelUuid);
        Assert.Equal("1.0", result.ManifestVersion);
        Assert.Equal("Test Model", result.Name);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(2, result.Creators.Length);
        Assert.Equal("Creator1", result.Creators[0]);
        Assert.Equal("Creator2", result.Creators[1]);
        Assert.Equal(LicenseType.ACML_1_0, result.LicenseType);
        Assert.Null(result.LicenseText);
        Assert.Equal(ModelType.Aivm, result.ModelType);
        Assert.Equal(ModelArchitecture.StyleBertVITS2, result.ModelArchitecture);
        Assert.Equal(ModelFormat.Safetensors, result.ModelFormat);
        Assert.Equal(116, result.TrainingEpochs);
        Assert.Equal(32000, result.TrainingSteps);
        Assert.Equal("1.0.1", result.Version);
        Assert.Equal(257639874, result.FileSize);
        Assert.Equal("51a69c4218b73218", result.Checksum);
        Assert.Equal(186, result.DownloadCount);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var modelFile = new ModelFile(
            Guid.Parse("a59cb814-0083-4369-8542-f51a29e72af7"),
            "1.0",
            "Test Model",
            "Test Description",
            new[] { "Creator1", "Creator2" },
            LicenseType.CC0,
            "Custom License Text",
            ModelType.Aivmx,
            ModelArchitecture.StyleBertVITS2_JPEx,
            ModelFormat.Onnx,
            100,
            25000,
            "2.0.0",
            123456789,
            "abc123def456",
            500,
            DateTime.Parse("2025-01-01T12:00:00Z"),
            DateTime.Parse("2025-01-02T12:00:00Z")
        );

        // Act
        var result = JsonSerializer.Serialize(modelFile);

        // Assert
        Assert.Contains("\"aivm_model_uuid\":\"a59cb814-0083-4369-8542-f51a29e72af7\"", result);
        Assert.Contains("\"manifest_version\":\"1.0\"", result);
        Assert.Contains("\"name\":\"Test Model\"", result);
        Assert.Contains("\"license_type\":\"CC0\"", result);
        Assert.Contains("\"model_type\":\"Aivmx\"", result);
        Assert.Contains("\"model_architecture\":\"Style-Bert-VITS2 (JP-Extra)\"", result);
        Assert.Contains("\"model_format\":\"Onnx\"", result);
    }

    [Fact]
    public void Should_HandleNullableProperties()
    {
        // Arrange
        string json = """
        {
            "aivm_model_uuid": "a59cb814-0083-4369-8542-f51a29e72af7",
            "manifest_version": "1.0",
            "name": "Test Model",
            "description": "Test Description",
            "creators": [],
            "license_type": "Internal",
            "license_text": null,
            "model_type": "AIVM",
            "model_architecture": "Style-Bert-VITS2",
            "model_format": "Safetensors",
            "training_epoches": null,
            "training_steps": null,
            "version": "1.0.0",
            "file_size": 123456,
            "checksum": "checksum123",
            "download_count": 0,
            "created_at": "2025-01-01T00:00:00Z",
            "updated_at": "2025-01-01T00:00:00Z"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<ModelFile>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.LicenseText);
        Assert.Null(result.TrainingEpochs);
        Assert.Null(result.TrainingSteps);
        Assert.Empty(result.Creators);
    }
}