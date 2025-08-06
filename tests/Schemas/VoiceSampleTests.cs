using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class VoiceSampleTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "audio_url": "https://example.com/audio.mp3",
            "transcript": "Sample voice transcript"
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<VoiceSample>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("https://example.com/audio.mp3", result.AudioUrl);
        Assert.Equal("Sample voice transcript", result.Transcript);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var voiceSample = new VoiceSample(
            "https://example.com/test.wav",
            "This is a test transcript"
        );

        // Act
        var result = JsonSerializer.Serialize(voiceSample);

        // Assert
        var expectedJson = """{"audio_url":"https://example.com/test.wav","transcript":"This is a test transcript"}""";
        Assert.Equal(expectedJson, result);
    }

    [Fact]
    public void Should_RoundTripSerializeDeserialize_Success()
    {
        // Arrange
        var originalVoiceSample = new VoiceSample(
            "https://example.com/original.mp3",
            "Original transcript text"
        );

        // Act
        var json = JsonSerializer.Serialize(originalVoiceSample);
        var deserializedVoiceSample = JsonSerializer.Deserialize<VoiceSample>(json);

        // Assert
        Assert.NotNull(deserializedVoiceSample);
        Assert.Equal(originalVoiceSample.AudioUrl, deserializedVoiceSample.AudioUrl);
        Assert.Equal(originalVoiceSample.Transcript, deserializedVoiceSample.Transcript);
    }

    [Fact]
    public void Should_HandleEmptyStrings()
    {
        // Arrange
        string json = """
        {
            "audio_url": "",
            "transcript": ""
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<VoiceSample>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("", result.AudioUrl);
        Assert.Equal("", result.Transcript);
    }
}