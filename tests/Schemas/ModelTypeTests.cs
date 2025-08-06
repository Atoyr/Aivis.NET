using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class ModelTypeTests
{
    [Theory]
    [InlineData("AIVM", ModelType.Aivm)]
    [InlineData("AIVMX", ModelType.Aivmx)]
    public void Should_DeserializeFromString_Success(string jsonValue, ModelType expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<ModelType>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(ModelType.Aivm, "Aivm")]
    [InlineData(ModelType.Aivmx, "Aivmx")]
    public void Should_SerializeToString_Success(ModelType value, string expected)
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
        string json = "\"InvalidType\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ModelType>(json));
    }
}