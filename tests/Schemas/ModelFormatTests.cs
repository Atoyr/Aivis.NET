using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class ModelFormatTests
{
    [Theory]
    [InlineData("Safetensors", ModelFormat.Safetensors)]
    [InlineData("ONNX", ModelFormat.Onnx)]
    public void Should_DeserializeFromString_Success(string jsonValue, ModelFormat expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<ModelFormat>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ModelFormat.Safetensors, "Safetensors")]
    [InlineData(ModelFormat.Onnx, "Onnx")]
    public void Should_SerializeToString_Success(ModelFormat value, string expected)
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
        string json = "\"InvalidFormat\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModelFormat>(json));
    }
}