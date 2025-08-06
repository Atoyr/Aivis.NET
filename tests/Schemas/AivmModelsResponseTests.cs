using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class AivmModelsResponseTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "total": 2,
            "aivm_models": [
                {
                    "aivm_model_uuid": "a59cb814-0083-4369-8542-f51a29e72af7",
                    "user": {
                        "handle": "test_user1",
                        "name": "Test User 1",
                        "description": "First test user",
                        "icon_url": "https://example.com/user1.jpg",
                        "account_type": "User",
                        "account_status": "Active",
                        "social_links": []
                    },
                    "name": "Model 1",
                    "description": "First model",
                    "detailed_description": "Detailed description 1",
                    "category": "OriginalCharacter",
                    "voice_timbre": "YouthfulFemale",
                    "visibility": "Public",
                    "is_tag_locked": false,
                    "model_files": [],
                    "tags": [],
                    "like_count": 10,
                    "is_liked": false,
                    "speakers": [],
                    "created_at": "2025-01-01T00:00:00Z",
                    "updated_at": "2025-01-01T00:00:00Z"
                },
                {
                    "aivm_model_uuid": "b59cb814-0083-4369-8542-f51a29e72af8",
                    "user": {
                        "handle": "test_user2",
                        "name": "Test User 2",
                        "description": "Second test user",
                        "icon_url": "https://example.com/user2.jpg",
                        "account_type": "Official",
                        "account_status": "Active",
                        "social_links": []
                    },
                    "name": "Model 2",
                    "description": "Second model",
                    "detailed_description": "Detailed description 2",
                    "category": "ExistingCharacter",
                    "voice_timbre": "AdultMale",
                    "visibility": "Private",
                    "is_tag_locked": true,
                    "model_files": [],
                    "tags": [],
                    "like_count": 25,
                    "is_liked": true,
                    "speakers": [],
                    "created_at": "2025-01-02T00:00:00Z",
                    "updated_at": "2025-01-02T00:00:00Z"
                }
            ]
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<AivmModelsResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Total);
        Assert.Equal(2, result.AivmModels.Count());
        
        var models = result.AivmModels.ToArray();
        
        // First model
        Assert.Equal(Guid.Parse("a59cb814-0083-4369-8542-f51a29e72af7"), models[0].AivmModelUuid);
        Assert.Equal("test_user1", models[0].User.Handle);
        Assert.Equal("Model 1", models[0].Name);
        Assert.Equal(Category.OriginalCharacter, models[0].Category);
        Assert.Equal(VoiceTimbre.YouthfulFemale, models[0].VoiceTimbre);
        Assert.Equal(Visibility.Public, models[0].Visibility);
        Assert.False(models[0].IsTagLocked);
        Assert.Equal(10, models[0].LikeCount);
        Assert.False(models[0].IsLiked);
        
        // Second model
        Assert.Equal(Guid.Parse("b59cb814-0083-4369-8542-f51a29e72af8"), models[1].AivmModelUuid);
        Assert.Equal("test_user2", models[1].User.Handle);
        Assert.Equal("Model 2", models[1].Name);
        Assert.Equal(Category.ExistingCharacter, models[1].Category);
        Assert.Equal(VoiceTimbre.AdultMale, models[1].VoiceTimbre);
        Assert.Equal(Visibility.Private, models[1].Visibility);
        Assert.True(models[1].IsTagLocked);
        Assert.Equal(25, models[1].LikeCount);
        Assert.True(models[1].IsLiked);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var userResponse = new UserResponse(
            "test_handle",
            "Test Name",
            "Test Description",
            "https://example.com/icon.jpg",
            AccountType.User,
            AccountStatus.Active,
            Array.Empty<SocialLink>()
        );

        var aivmModelResponse = new AivmModelResponse(
            Guid.Parse("a59cb814-0083-4369-8542-f51a29e72af7"),
            userResponse,
            "Test Model",
            "Test Description",
            "Detailed Description",
            Category.OriginalCharacter,
            VoiceTimbre.YouthfulFemale,
            Visibility.Public,
            false,
            Array.Empty<ModelFile>(),
            Array.Empty<Tag>(),
            5,
            true,
            Array.Empty<Speaker>(),
            DateTime.Parse("2025-01-01T00:00:00Z"),
            DateTime.Parse("2025-01-01T00:00:00Z")
        );

        var aivmModelsResponse = new AivmModelsResponse(
            1,
            new[] { aivmModelResponse }
        );

        // Act
        var result = JsonSerializer.Serialize(aivmModelsResponse);

        // Assert
        Assert.Contains("\"total\":1", result);
        Assert.Contains("\"aivm_models\":", result);
        Assert.Contains("\"aivm_model_uuid\":\"a59cb814-0083-4369-8542-f51a29e72af7\"", result);
        Assert.Contains("\"name\":\"Test Model\"", result);
    }

    [Fact]
    public void Should_HandleEmptyModels()
    {
        // Arrange
        string json = """
        {
            "total": 0,
            "aivm_models": []
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<AivmModelsResponse>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Total);
        Assert.Empty(result.AivmModels);
    }

    [Fact]
    public void Should_RoundTripSerializeDeserialize_Success()
    {
        // Arrange
        var originalUserResponse = new UserResponse(
            "original_handle",
            "Original Name",
            "Original Description",
            "https://example.com/original.jpg",
            AccountType.Admin,
            AccountStatus.Active,
            Array.Empty<SocialLink>()
        );

        var originalModelResponse = new AivmModelResponse(
            Guid.Parse("12345678-1234-1234-1234-123456789012"),
            originalUserResponse,
            "Original Model",
            "Original Description",
            "Original Detailed Description",
            Category.Other,
            VoiceTimbre.Neutral,
            Visibility.AdminHidden,
            true,
            Array.Empty<ModelFile>(),
            Array.Empty<Tag>(),
            100,
            false,
            Array.Empty<Speaker>(),
            DateTime.Parse("2025-01-01T12:00:00Z"),
            DateTime.Parse("2025-01-02T12:00:00Z")
        );

        var originalResponse = new AivmModelsResponse(
            1,
            new[] { originalModelResponse }
        );

        // Act
        var json = JsonSerializer.Serialize(originalResponse);
        var deserializedResponse = JsonSerializer.Deserialize<AivmModelsResponse>(json);

        // Assert
        Assert.NotNull(deserializedResponse);
        Assert.Equal(originalResponse.Total, deserializedResponse.Total);
        Assert.Equal(originalResponse.AivmModels.Count(), deserializedResponse.AivmModels.Count());
        
        var originalModel = originalResponse.AivmModels.First();
        var deserializedModel = deserializedResponse.AivmModels.First();
        
        Assert.Equal(originalModel.AivmModelUuid, deserializedModel.AivmModelUuid);
        Assert.Equal(originalModel.Name, deserializedModel.Name);
        Assert.Equal(originalModel.User.Handle, deserializedModel.User.Handle);
    }
}