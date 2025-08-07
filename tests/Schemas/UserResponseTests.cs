using System.Text.Json;

using Xunit;

namespace Aivis.Schemas.Tests;

public class UserResponseTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "handle": "test_user",
            "name": "Test User",
            "description": "This is a test user",
            "icon_url": "https://example.com/icon.jpg",
            "account_type": "User",
            "account_status": "Active",
            "social_links": [
                {
                    "type": "Twitter",
                    "url": "https://twitter.com/test_user"
                },
                {
                    "type": "GitHub",
                    "url": "https://github.com/test_user"
                }
            ]
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<UserResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test_user", result.Handle);
        Assert.Equal("Test User", result.Name);
        Assert.Equal("This is a test user", result.Description);
        Assert.Equal("https://example.com/icon.jpg", result.IconUrl);
        Assert.Equal(AccountType.User, result.AccountType);
        Assert.Equal(AccountStatus.Active, result.AccountStatus);
        Assert.Equal(2, result.SocialLinks.Length);
        Assert.Equal(SocialLinkType.Twitter, result.SocialLinks[0].Type);
        Assert.Equal("https://twitter.com/test_user", result.SocialLinks[0].Url);
        Assert.Equal(SocialLinkType.GitHub, result.SocialLinks[1].Type);
        Assert.Equal("https://github.com/test_user", result.SocialLinks[1].Url);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var socialLinks = new[]
        {
            new SocialLink(SocialLinkType.Twitter, "https://twitter.com/test"),
            new SocialLink(SocialLinkType.Website, "https://example.com")
        };

        var userResponse = new UserResponse(
            "test_handle",
            "Test Name",
            "Test Description",
            "https://example.com/avatar.png",
            AccountType.Official,
            AccountStatus.Active,
            socialLinks
        );

        // Act
        var result = JsonSerializer.Serialize(userResponse);

        // Assert
        Assert.Contains("\"handle\":\"test_handle\"", result);
        Assert.Contains("\"name\":\"Test Name\"", result);
        Assert.Contains("\"description\":\"Test Description\"", result);
        Assert.Contains("\"icon_url\":\"https://example.com/avatar.png\"", result);
        Assert.Contains("\"account_type\":\"Official\"", result);
        Assert.Contains("\"account_status\":\"Active\"", result);
        Assert.Contains("\"social_links\":", result);
    }

    [Fact]
    public void Should_RoundTripSerializeDeserialize_Success()
    {
        // Arrange
        var originalSocialLinks = new[]
        {
            new SocialLink(SocialLinkType.GitHub, "https://github.com/original"),
            new SocialLink(SocialLinkType.YouTube, "https://youtube.com/original")
        };

        var originalUserResponse = new UserResponse(
            "original_handle",
            "Original Name",
            "Original Description",
            "https://example.com/original.jpg",
            AccountType.Admin,
            AccountStatus.Suspended,
            originalSocialLinks
        );

        // Act
        var json = JsonSerializer.Serialize(originalUserResponse);
        var deserializedUserResponse = JsonSerializer.Deserialize<UserResponse>(json);

        // Assert
        Assert.NotNull(deserializedUserResponse);
        Assert.Equal(originalUserResponse.Handle, deserializedUserResponse.Handle);
        Assert.Equal(originalUserResponse.Name, deserializedUserResponse.Name);
        Assert.Equal(originalUserResponse.Description, deserializedUserResponse.Description);
        Assert.Equal(originalUserResponse.IconUrl, deserializedUserResponse.IconUrl);
        Assert.Equal(originalUserResponse.AccountType, deserializedUserResponse.AccountType);
        Assert.Equal(originalUserResponse.AccountStatus, deserializedUserResponse.AccountStatus);
        Assert.Equal(originalUserResponse.SocialLinks.Length, deserializedUserResponse.SocialLinks.Length);

        for (int i = 0; i < originalUserResponse.SocialLinks.Length; i++)
        {
            Assert.Equal(originalUserResponse.SocialLinks[i].Type, deserializedUserResponse.SocialLinks[i].Type);
            Assert.Equal(originalUserResponse.SocialLinks[i].Url, deserializedUserResponse.SocialLinks[i].Url);
        }
    }

    [Fact]
    public void Should_HandleEmptySocialLinks()
    {
        // Arrange
        string json = """
        {
            "handle": "no_social",
            "name": "No Social User",
            "description": "User with no social links",
            "icon_url": "https://example.com/icon.jpg",
            "account_type": "User",
            "account_status": "Active",
            "social_links": []
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<UserResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("no_social", result.Handle);
        Assert.Empty(result.SocialLinks);
    }
}