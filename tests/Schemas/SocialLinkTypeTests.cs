using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class SocialLinkTypeTests
{
    [Theory]
    [InlineData("Twitter", SocialLinkType.Twitter)]
    [InlineData("Bluesky", SocialLinkType.Bluesky)]
    [InlineData("Misskey", SocialLinkType.Misskey)]
    [InlineData("YouTube", SocialLinkType.YouTube)]
    [InlineData("Niconico", SocialLinkType.Niconico)]
    [InlineData("Instagram", SocialLinkType.Instagram)]
    [InlineData("TikTok", SocialLinkType.TikTok)]
    [InlineData("Facebook", SocialLinkType.Facebook)]
    [InlineData("GitHub", SocialLinkType.GitHub)]
    [InlineData("HuggingFace", SocialLinkType.HuggingFace)]
    [InlineData("Website", SocialLinkType.Website)]
    public void Should_DeserializeFromString_Success(string jsonValue, SocialLinkType expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<SocialLinkType>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(SocialLinkType.Twitter, "Twitter")]
    [InlineData(SocialLinkType.Bluesky, "Bluesky")]
    [InlineData(SocialLinkType.Misskey, "Misskey")]
    [InlineData(SocialLinkType.YouTube, "YouTube")]
    [InlineData(SocialLinkType.Niconico, "Niconico")]
    [InlineData(SocialLinkType.Instagram, "Instagram")]
    [InlineData(SocialLinkType.TikTok, "TikTok")]
    [InlineData(SocialLinkType.Facebook, "Facebook")]
    [InlineData(SocialLinkType.GitHub, "GitHub")]
    [InlineData(SocialLinkType.Website, "Website")]
    public void Should_SerializeToString_Success(SocialLinkType value, string expected)
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
        string json = "\"InvalidSocialLink\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SocialLinkType>(json));
    }

    [Fact]
    public void Should_HandleSameEnumValues_GitHubAndHuggingFace()
    {
        // Both GitHub and HuggingFace have the same enum value (8)
        // When serializing, it should serialize to the first enum name (GitHub)
        
        // Act
        var githubResult = JsonSerializer.Serialize(SocialLinkType.GitHub);
        var huggingFaceResult = JsonSerializer.Serialize(SocialLinkType.HuggingFace);

        // Assert
        Assert.Equal("\"GitHub\"", githubResult);
        Assert.Equal("\"GitHub\"", huggingFaceResult); // Both serialize to "GitHub"
        
        // Verify they have the same underlying value
        Assert.Equal((int)SocialLinkType.GitHub, (int)SocialLinkType.HuggingFace);
    }
}