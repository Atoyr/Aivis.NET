using System.Text.Json;

using Xunit;

namespace Aivis.Schemas.Tests;

public class TagTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "name": "Popular"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<Tag>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Popular", result.Name);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var tag = new Tag("Anime");

        // Act
        var result = JsonSerializer.Serialize(tag);

        // Assert
        var expectedJson = """{"name":"Anime"}""";
        Assert.Equal(expectedJson, result);
    }

    [Fact]
    public void Should_RoundTripSerializeDeserialize_Success()
    {
        // Arrange
        var originalTag = new Tag("OriginalTag");

        // Act
        var json = JsonSerializer.Serialize(originalTag);
        var deserializedTag = JsonSerializer.Deserialize<Tag>(json);

        // Assert
        Assert.NotNull(deserializedTag);
        Assert.Equal(originalTag.Name, deserializedTag.Name);
    }

    [Fact]
    public void Should_HandleEmptyName()
    {
        // Arrange
        string json = """
        {
            "name": ""
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<Tag>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("", result.Name);
    }

    [Theory]
    [InlineData("Normal")]
    [InlineData("With Spaces")]
    [InlineData("With-Dashes")]
    [InlineData("WithNumbers123")]
    [InlineData("特殊文字")]
    public void Should_HandleVariousTagNames(string tagName)
    {
        // Arrange
        var tag = new Tag(tagName);

        // Act
        var json = JsonSerializer.Serialize(tag);
        var deserializedTag = JsonSerializer.Deserialize<Tag>(json);

        // Assert
        Assert.NotNull(deserializedTag);
        Assert.Equal(tagName, deserializedTag.Name);
    }
}