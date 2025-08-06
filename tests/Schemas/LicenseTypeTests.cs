using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class LicenseTypeTests
{
    [Theory]
    [InlineData("ACML 1.0", LicenseType.ACML_1_0)]
    [InlineData("ACML-NC 1.0", LicenseType.ACML_NC_1_0)]
    [InlineData("CC0", LicenseType.CC0)]
    [InlineData("Custom", LicenseType.Custom)]
    [InlineData("Internal", LicenseType.Internal)]
    public void Should_DeserializeFromString_Success(string jsonValue, LicenseType expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(LicenseType.ACML_1_0, "ACML 1.0")]
    [InlineData(LicenseType.ACML_NC_1_0, "ACML-NC 1.0")]
    [InlineData(LicenseType.CC0, "CC0")]
    [InlineData(LicenseType.Custom, "Custom")]
    [InlineData(LicenseType.Internal, "Internal")]
    public void Should_SerializeToString_Success(LicenseType value, string expected)
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
        string json = "\"UnknownLicense\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<LicenseType>(json));
    }

    [Fact]
    public void Should_HandleSpaceInEnumMemberValue()
    {
        // Arrange
        string json = "\"ACML 1.0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json);

        // Assert
        Assert.Equal(LicenseType.ACML_1_0, result);
    }

    [Fact]
    public void Should_HandleHyphenInEnumMemberValue()
    {
        // Arrange
        string json = "\"ACML-NC 1.0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json);

        // Assert
        Assert.Equal(LicenseType.ACML_NC_1_0, result);
    }
}