using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class CategoryTests
{
    [Theory]
    [InlineData("ExistingCharacter", Category.ExistingCharacter)]
    [InlineData("OriginalCharacter", Category.OriginalCharacter)]
    [InlineData("LivingPerson", Category.LivingPerson)]
    [InlineData("DeceasedPerson", Category.DeceasedPerson)]
    [InlineData("FictionalPerson", Category.FictionalPerson)]
    [InlineData("Other", Category.Other)]
    public void Should_DeserializeFromString_Success(string jsonValue, Category expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<Category>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Category.ExistingCharacter, "ExistingCharacter")]
    [InlineData(Category.OriginalCharacter, "OriginalCharacter")]
    [InlineData(Category.LivingPerson, "LivingPerson")]
    [InlineData(Category.DeceasedPerson, "DeceasedPerson")]
    [InlineData(Category.FictionalPerson, "FictionalPerson")]
    [InlineData(Category.Other, "Other")]
    public void Should_SerializeToString_Success(Category value, string expected)
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
        string json = "\"InvalidCategory\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Category>(json));
    }
}