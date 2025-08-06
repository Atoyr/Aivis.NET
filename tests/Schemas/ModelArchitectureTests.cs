using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class ModelArchitectureTests
{
    [Theory]
    [InlineData("Style-Bert-VITS2", ModelArchitecture.StyleBertVITS2)]
    [InlineData("Style-Bert-VITS2 (JP-Extra)", ModelArchitecture.StyleBertVITS2_JPEx)]
    public void Should_DeserializeFromString_Success(string jsonValue, ModelArchitecture expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<ModelArchitecture>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ModelArchitecture.StyleBertVITS2, "Style-Bert-VITS2")]
    [InlineData(ModelArchitecture.StyleBertVITS2_JPEx, "Style-Bert-VITS2 (JP-Extra)")]
    public void Should_SerializeToString_Success(ModelArchitecture value, string expected)
    {
        // Act
        var result = JsonSerializer.Serialize(value);

        // Assert
        Assert.Equal($"\"{expected}\"", result);
    }

    [Fact]
    public void Should_ThrowException_WhenInvalidValue()
    {
        // Arrange
        string json = "\"InvalidArchitecture\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModelArchitecture>(json));
    }

    [Fact]
    public void Should_HandleComplexEnumMemberValue()
    {
        // Arrange
        string json = "\"Style-Bert-VITS2 (JP-Extra)\"";

        // Act
        var result = JsonSerializer.Deserialize<ModelArchitecture>(json);

        // Assert
        Assert.Equal(ModelArchitecture.StyleBertVITS2_JPEx, result);
    }
}