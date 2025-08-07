using System.Text.Json;

using Xunit;

namespace Aivis.Schemas.Tests;

public class EnumMemberStringEnumConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters = { new EnumMemberStringEnumConverter() }
    };

    [Fact]
    public void Should_DeserializeLicenseTypeWithSpace_Success()
    {
        // Arrange
        string json = "\"ACML 1.0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json, _options);

        // Assert
        Assert.Equal(LicenseType.ACML_1_0, result);
    }

    [Fact]
    public void Should_DeserializeLicenseTypeWithHyphen_Success()
    {
        // Arrange
        string json = "\"ACML-NC 1.0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json, _options);

        // Assert
        Assert.Equal(LicenseType.ACML_NC_1_0, result);
    }

    [Fact]
    public void Should_SerializeLicenseType_Success()
    {
        // Act
        var result = JsonSerializer.Serialize(LicenseType.ACML_1_0, _options);

        // Assert
        Assert.Equal("\"ACML 1.0\"", result);
    }

    [Fact]
    public void Should_DeserializeModelArchitectureWithComplexValue_Success()
    {
        // Arrange
        string json = "\"Style-Bert-VITS2 (JP-Extra)\"";

        // Act
        var result = JsonSerializer.Deserialize<ModelArchitecture>(json, _options);

        // Assert
        Assert.Equal(ModelArchitecture.StyleBertVITS2_JPEx, result);
    }

    [Fact]
    public void Should_SerializeModelArchitecture_Success()
    {
        // Act
        var result = JsonSerializer.Serialize(ModelArchitecture.StyleBertVITS2_JPEx, _options);

        // Assert
        Assert.Equal("\"Style-Bert-VITS2 (JP-Extra)\"", result);
    }

    [Fact]
    public void Should_ThrowException_WhenInvalidEnumValue()
    {
        // Arrange
        string json = "\"InvalidLicenseType\"";

        // Act & Assert
        var exception = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<LicenseType>(json, _options));

        Assert.Contains("Unable to convert", exception.Message);
        Assert.Contains("InvalidLicenseType", exception.Message);
    }

    [Fact]
    public void Should_ThrowException_WhenNullValue()
    {
        // Arrange
        string json = "null";

        // Act & Assert
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<LicenseType>(json, _options));
    }

    [Fact]
    public void Should_ThrowException_WhenNonStringValue()
    {
        // Arrange
        string json = "123";

        // Act & Assert
        var exception = Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<LicenseType>(json, _options));

        Assert.Contains("Expected string value", exception.Message);
    }

    [Fact]
    public void Should_HandleFallbackToFieldName()
    {
        // Arrange - Using field name instead of EnumMember value
        string json = "\"ACML_1_0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json, _options);

        // Assert
        Assert.Equal(LicenseType.ACML_1_0, result);
    }

    [Fact]
    public void Should_HandleFallbackToToString()
    {
        // Arrange - Using ToString() result
        string json = "\"ACML_1_0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json, _options);

        // Assert
        Assert.Equal(LicenseType.ACML_1_0, result);
    }

    [Theory]
    [InlineData(LicenseType.ACML_1_0, "\"ACML 1.0\"")]
    [InlineData(LicenseType.ACML_NC_1_0, "\"ACML-NC 1.0\"")]
    [InlineData(LicenseType.CC0, "\"CC0\"")]
    [InlineData(LicenseType.Custom, "\"Custom\"")]
    [InlineData(LicenseType.Internal, "\"Internal\"")]
    public void Should_RoundTripSerializeDeserialize_AllLicenseTypes(LicenseType licenseType, string expectedJson)
    {
        // Act - Serialize
        var json = JsonSerializer.Serialize(licenseType, _options);

        // Assert - Check serialization
        Assert.Equal(expectedJson, json);

        // Act - Deserialize
        var deserialized = JsonSerializer.Deserialize<LicenseType>(json, _options);

        // Assert - Check deserialization
        Assert.Equal(licenseType, deserialized);
    }

    [Fact]
    public void Should_WorkWithGenericConverter()
    {
        // Arrange
        var specificOptions = new JsonSerializerOptions
        {
            Converters = { new EnumMemberStringEnumConverter<LicenseType>() }
        };
        string json = "\"ACML 1.0\"";

        // Act
        var result = JsonSerializer.Deserialize<LicenseType>(json, specificOptions);

        // Assert
        Assert.Equal(LicenseType.ACML_1_0, result);
    }

    [Fact]
    public void Should_SerializeWithGenericConverter()
    {
        // Arrange
        var specificOptions = new JsonSerializerOptions
        {
            Converters = { new EnumMemberStringEnumConverter<LicenseType>() }
        };

        // Act
        var result = JsonSerializer.Serialize(LicenseType.ACML_NC_1_0, specificOptions);

        // Assert
        Assert.Equal("\"ACML-NC 1.0\"", result);
    }
}