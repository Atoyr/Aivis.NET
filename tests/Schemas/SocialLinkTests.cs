using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class SocialLinkTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "type": "Twitter",
            "url": "https://twitter.com/example"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<SocialLink>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SocialLinkType.Twitter, result.Type);
        Assert.Equal("https://twitter.com/example", result.Url);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var socialLink = new SocialLink(SocialLinkType.GitHub, "https://github.com/example");

        // Act
        var result = JsonSerializer.Serialize(socialLink);

        // Assert
        var expectedJson = """{"type":"GitHub","url":"https://github.com/example"}""";
        Assert.Equal(expectedJson, result);
    }

    [Theory]
    [InlineData("Twitter", "https://twitter.com/test")]
    [InlineData("GitHub", "https://github.com/test")]
    [InlineData("Website", "https://example.com")]
    public void Should_RoundTripSerializeDeserialize_Success(string typeValue, string url)
    {
        // Arrange
        var socialLinkType = Enum.Parse<SocialLinkType>(typeValue);
        var originalSocialLink = new SocialLink(socialLinkType, url);

        // Act
        var json = JsonSerializer.Serialize(originalSocialLink);
        var deserializedSocialLink = JsonSerializer.Deserialize<SocialLink>(json);

        // Assert
        Assert.NotNull(deserializedSocialLink);
        Assert.Equal(originalSocialLink.Type, deserializedSocialLink.Type);
        Assert.Equal(originalSocialLink.Url, deserializedSocialLink.Url);
    }

    [Fact]
    public void Should_HandleEmptyUrl()
    {
        // Arrange
        string json = """
        {
            "type": "Website",
            "url": ""
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<SocialLink>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SocialLinkType.Website, result.Type);
        Assert.Equal("", result.Url);
    }
}