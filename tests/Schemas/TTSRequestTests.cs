using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class TTSRequestTests
{
    [Fact]
    public void Should_CreateWithValidModelUuidAndText()
    {
        // Arrange
        var modelUuid = "a59cb814-0083-4369-8542-f51a29e72af7";
        var text = "Hello, world!";

        // Act
        var request = new TTSRequest(modelUuid, text);

        // Assert
        Assert.Equal(modelUuid, request.ModelUuid);
        Assert.Equal(text, request.Text);
        Assert.Null(request.SpeakerUuid);
        Assert.Equal(0, request.StyleId);
        Assert.Null(request.UserDictionaryUuid);
        Assert.True(request.UseSsml);
        Assert.Equal("mp3", request.OutputFormat);
    }

    [Fact]
    public void Should_ThrowException_WhenInvalidModelUuid()
    {
        // Arrange
        var invalidUuid = "invalid-uuid";
        var text = "Hello, world!";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new TTSRequest(invalidUuid, text));
    }

    [Fact]
    public void Should_SetValidSpeakerUuid()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");
        var speakerUuid = "b59cb814-0083-4369-8542-f51a29e72af8";

        // Act
        request.SpeakerUuid = speakerUuid;

        // Assert
        Assert.Equal(speakerUuid, request.SpeakerUuid);
    }

    [Fact]
    public void Should_ThrowException_WhenInvalidSpeakerUuid()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.SpeakerUuid = "invalid-uuid");
    }

    [Fact]
    public void Should_SetNullSpeakerUuid()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");
        request.SpeakerUuid = "b59cb814-0083-4369-8542-f51a29e72af8";

        // Act
        request.SpeakerUuid = null;

        // Assert
        Assert.Null(request.SpeakerUuid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(31)]
    public void Should_SetValidStyleId(int styleId)
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");

        // Act
        request.StyleId = styleId;

        // Assert
        Assert.Equal(styleId, request.StyleId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    [InlineData(100)]
    public void Should_ThrowException_WhenInvalidStyleId(int invalidStyleId)
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => request.StyleId = invalidStyleId);
    }

    [Fact]
    public void Should_SetValidUserDictionaryUuid()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");
        var dictionaryUuid = "c59cb814-0083-4369-8542-f51a29e72af9";

        // Act
        request.UserDictionaryUuid = dictionaryUuid;

        // Assert
        Assert.Equal(dictionaryUuid, request.UserDictionaryUuid);
    }

    [Fact]
    public void Should_ThrowException_WhenInvalidUserDictionaryUuid()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => request.UserDictionaryUuid = "invalid-uuid");
    }

    [Fact]
    public void Should_SetNullUserDictionaryUuid()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");
        request.UserDictionaryUuid = "c59cb814-0083-4369-8542-f51a29e72af9";

        // Act
        request.UserDictionaryUuid = null;

        // Assert
        Assert.Null(request.UserDictionaryUuid);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "Hello, world!")
        {
            SpeakerUuid = "b59cb814-0083-4369-8542-f51a29e72af8",
            StyleId = 1,
            UserDictionaryUuid = "c59cb814-0083-4369-8542-f51a29e72af9",
            UseSsml = false,
            OutputFormat = "wav"
        };

        // Act
        var json = JsonSerializer.Serialize(request);

        // Assert
        Assert.Contains("\"model_uuid\":\"a59cb814-0083-4369-8542-f51a29e72af7\"", json);
        Assert.Contains("\"speaker_uuid\":\"b59cb814-0083-4369-8542-f51a29e72af8\"", json);
        Assert.Contains("\"style_id\":1", json);
        Assert.Contains("\"user_dictionary_uuid\":\"c59cb814-0083-4369-8542-f51a29e72af9\"", json);
        Assert.Contains("\"text\":\"Hello, world!\"", json);
        Assert.Contains("\"use_ssml\":false", json);
        Assert.Contains("\"output_format\":\"wav\"", json);
    }

    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "model_uuid": "a59cb814-0083-4369-8542-f51a29e72af7",
            "speaker_uuid": "b59cb814-0083-4369-8542-f51a29e72af8",
            "style_id": 2,
            "user_dictionary_uuid": null,
            "text": "Test text",
            "use_ssml": true,
            "output_format": "flac"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<TTSRequest>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("a59cb814-0083-4369-8542-f51a29e72af7", result.ModelUuid);
        Assert.Equal("b59cb814-0083-4369-8542-f51a29e72af8", result.SpeakerUuid);
        Assert.Equal(2, result.StyleId);
        Assert.Null(result.UserDictionaryUuid);
        Assert.Equal("Test text", result.Text);
        Assert.True(result.UseSsml);
        Assert.Equal("flac", result.OutputFormat);
    }

    [Fact]
    public void Should_RoundTripSerializeDeserialize_Success()
    {
        // Arrange
        var originalRequest = new TTSRequest("12345678-1234-1234-1234-123456789012", "Original text")
        {
            SpeakerUuid = "87654321-4321-4321-4321-210987654321",
            StyleId = 5,
            UserDictionaryUuid = "abcdefab-abcd-abcd-abcd-abcdefabcdef",
            UseSsml = false,
            OutputFormat = "aac"
        };

        // Act
        var json = JsonSerializer.Serialize(originalRequest);
        var deserializedRequest = JsonSerializer.Deserialize<TTSRequest>(json);

        // Assert
        Assert.NotNull(deserializedRequest);
        Assert.Equal(originalRequest.ModelUuid, deserializedRequest.ModelUuid);
        Assert.Equal(originalRequest.SpeakerUuid, deserializedRequest.SpeakerUuid);
        Assert.Equal(originalRequest.StyleId, deserializedRequest.StyleId);
        Assert.Equal(originalRequest.UserDictionaryUuid, deserializedRequest.UserDictionaryUuid);
        Assert.Equal(originalRequest.Text, deserializedRequest.Text);
        Assert.Equal(originalRequest.UseSsml, deserializedRequest.UseSsml);
        Assert.Equal(originalRequest.OutputFormat, deserializedRequest.OutputFormat);
    }

    [Theory]
    [InlineData("wav")]
    [InlineData("flac")]
    [InlineData("mp3")]
    [InlineData("aac")]
    [InlineData("opus")]
    public void Should_HandleDifferentOutputFormats(string format)
    {
        // Arrange
        var request = new TTSRequest("a59cb814-0083-4369-8542-f51a29e72af7", "test");

        // Act
        request.OutputFormat = format;

        // Assert
        Assert.Equal(format, request.OutputFormat);
    }
}