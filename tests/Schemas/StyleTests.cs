using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class StyleTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "name": "Normal",
            "icon_url": "https://example.com/style_icon.png",
            "local_id": 0,
            "voice_samples": [
                {
                    "audio_url": "https://example.com/sample1.mp3",
                    "transcript": "Sample 1 transcript"
                },
                {
                    "audio_url": "https://example.com/sample2.mp3",
                    "transcript": "Sample 2 transcript"
                }
            ]
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<Style>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Normal", result.Name);
        Assert.Equal("https://example.com/style_icon.png", result.IconUrl);
        Assert.Equal(0, result.LocalId);
        Assert.Equal(2, result.VoiceSamples.Length);
        Assert.Equal("https://example.com/sample1.mp3", result.VoiceSamples[0].AudioUrl);
        Assert.Equal("Sample 1 transcript", result.VoiceSamples[0].Transcript);
        Assert.Equal("https://example.com/sample2.mp3", result.VoiceSamples[1].AudioUrl);
        Assert.Equal("Sample 2 transcript", result.VoiceSamples[1].Transcript);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var voiceSamples = new[]
        {
            new VoiceSample("https://example.com/test1.wav", "Test transcript 1"),
            new VoiceSample("https://example.com/test2.wav", "Test transcript 2")
        };

        var style = new Style(
            "Happy",
            "https://example.com/happy_icon.png",
            1,
            voiceSamples
        );

        // Act
        var result = JsonSerializer.Serialize(style);

        // Assert
        Assert.Contains("\"name\":\"Happy\"", result);
        Assert.Contains("\"icon_url\":\"https://example.com/happy_icon.png\"", result);
        Assert.Contains("\"local_id\":1", result);
        Assert.Contains("\"voice_samples\":", result);
    }

    [Fact]
    public void Should_HandleNullIconUrl()
    {
        // Arrange
        string json = """
        {
            "name": "Default",
            "icon_url": null,
            "local_id": 0,
            "voice_samples": []
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<Style>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Default", result.Name);
        Assert.Null(result.IconUrl);
        Assert.Equal(0, result.LocalId);
        Assert.Empty(result.VoiceSamples);
    }

    [Fact]
    public void Should_HandleEmptyVoiceSamples()
    {
        // Arrange
        var style = new Style(
            "Silent",
            null,
            2,
            Array.Empty<VoiceSample>()
        );

        // Act
        var json = JsonSerializer.Serialize(style);
        var deserializedStyle = JsonSerializer.Deserialize<Style>(json);

        // Assert
        Assert.NotNull(deserializedStyle);
        Assert.Equal("Silent", deserializedStyle.Name);
        Assert.Null(deserializedStyle.IconUrl);
        Assert.Equal(2, deserializedStyle.LocalId);
        Assert.Empty(deserializedStyle.VoiceSamples);
    }
}