using System.Text.Json;

using Xunit;

namespace Aivis.Schemas.Tests;

public class VisibilityTests
{
    [Theory]
    [InlineData("Public", Visibility.Public)]
    [InlineData("Private", Visibility.Private)]
    [InlineData("AdminHidden", Visibility.AdminHidden)]
    public void Should_DeserializeFromString_Success(string jsonValue, Visibility expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<Visibility>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Visibility.Public, "Public")]
    [InlineData(Visibility.Private, "Private")]
    [InlineData(Visibility.AdminHidden, "AdminHidden")]
    public void Should_SerializeToString_Success(Visibility value, string expected)
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
        string json = "\"InvalidVisibility\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Visibility>(json));
    }
}