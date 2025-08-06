using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class SpeakerTests
{
    [Fact]
    public void Should_DeserializeFromJson_Success()
    {
        // Arrange
        string json = """
        {
            "aivm_speaker_uuid": "550e8400-e29b-41d4-a716-446655440000",
            "name": "Test Speaker",
            "icon_url": "https://example.com/speaker_icon.png",
            "supported_languages": ["ja", "en"],
            "local_id": 1,
            "styles": [
                {
                    "name": "Normal",
                    "icon_url": "https://example.com/normal_icon.png",
                    "local_id": 0,
                    "voice_samples": []
                },
                {
                    "name": "Happy",
                    "icon_url": null,
                    "local_id": 1,
                    "voice_samples": [
                        {
                            "audio_url": "https://example.com/happy_sample.mp3",
                            "transcript": "Happy voice sample"
                        }
                    ]
                }
            ]
        }
        """;

        // Act
        var result = JsonSerializer.Deserialize<Speaker>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), result.AivmSpeakerUuid);
        Assert.Equal("Test Speaker", result.Name);
        Assert.Equal("https://example.com/speaker_icon.png", result.IconUrl);
        Assert.Equal(2, result.SupportedLanguages.Length);
        Assert.Equal("ja", result.SupportedLanguages[0]);
        Assert.Equal("en", result.SupportedLanguages[1]);
        Assert.Equal(1, result.LocalId);
        Assert.Equal(2, result.Styles.Length);
        
        // First style
        Assert.Equal("Normal", result.Styles[0].Name);
        Assert.Equal("https://example.com/normal_icon.png", result.Styles[0].IconUrl);
        Assert.Equal(0, result.Styles[0].LocalId);
        Assert.Empty(result.Styles[0].VoiceSamples);
        
        // Second style
        Assert.Equal("Happy", result.Styles[1].Name);
        Assert.Null(result.Styles[1].IconUrl);
        Assert.Equal(1, result.Styles[1].LocalId);
        Assert.Single(result.Styles[1].VoiceSamples);
        Assert.Equal("https://example.com/happy_sample.mp3", result.Styles[1].VoiceSamples[0].AudioUrl);
        Assert.Equal("Happy voice sample", result.Styles[1].VoiceSamples[0].Transcript);
    }

    [Fact]
    public void Should_SerializeToJson_Success()
    {
        // Arrange
        var styles = new[]
        {
            new Style("Normal", "https://example.com/normal.png", 0, Array.Empty<VoiceSample>()),
            new Style("Excited", null, 1, new[]
            {
                new VoiceSample("https://example.com/excited.mp3", "Excited sample")
            })
        };

        var speaker = new Speaker(
            Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
            "Test Speaker",
            "https://example.com/speaker.png",
            new[] { "ja", "en", "ko" },
            2,
            styles
        );

        // Act
        var result = JsonSerializer.Serialize(speaker);

        // Assert
        Assert.Contains("\"aivm_speaker_uuid\":\"550e8400-e29b-41d4-a716-446655440000\"", result);
        Assert.Contains("\"name\":\"Test Speaker\"", result);
        Assert.Contains("\"icon_url\":\"https://example.com/speaker.png\"", result);
        Assert.Contains("\"supported_languages\":[\"ja\",\"en\",\"ko\"]", result);
        Assert.Contains("\"local_id\":2", result);
        Assert.Contains("\"styles\":", result);
    }

    [Fact]
    public void Should_HandleEmptyLanguagesAndStyles()
    {
        // Arrange
        var speaker = new Speaker(
            Guid.NewGuid(),
            "Minimal Speaker",
            "https://example.com/minimal.png",
            Array.Empty<string>(),
            0,
            Array.Empty<Style>()
        );

        // Act
        var json = JsonSerializer.Serialize(speaker);
        var deserializedSpeaker = JsonSerializer.Deserialize<Speaker>(json);

        // Assert
        Assert.NotNull(deserializedSpeaker);
        Assert.Equal("Minimal Speaker", deserializedSpeaker.Name);
        Assert.Empty(deserializedSpeaker.SupportedLanguages);
        Assert.Empty(deserializedSpeaker.Styles);
    }

    [Fact]
    public void Should_RoundTripSerializeDeserialize_Success()
    {
        // Arrange
        var originalStyles = new[]
        {
            new Style("Style1", "https://example.com/style1.png", 0, new[]
            {
                new VoiceSample("https://example.com/sample1.mp3", "Sample 1")
            })
        };

        var originalSpeaker = new Speaker(
            Guid.Parse("12345678-1234-1234-1234-123456789012"),
            "Round Trip Speaker",
            "https://example.com/roundtrip.png",
            new[] { "ja" },
            5,
            originalStyles
        );

        // Act
        var json = JsonSerializer.Serialize(originalSpeaker);
        var deserializedSpeaker = JsonSerializer.Deserialize<Speaker>(json);

        // Assert
        Assert.NotNull(deserializedSpeaker);
        Assert.Equal(originalSpeaker.AivmSpeakerUuid, deserializedSpeaker.AivmSpeakerUuid);
        Assert.Equal(originalSpeaker.Name, deserializedSpeaker.Name);
        Assert.Equal(originalSpeaker.IconUrl, deserializedSpeaker.IconUrl);
        Assert.Equal(originalSpeaker.SupportedLanguages.Length, deserializedSpeaker.SupportedLanguages.Length);
        Assert.Equal(originalSpeaker.LocalId, deserializedSpeaker.LocalId);
        Assert.Equal(originalSpeaker.Styles.Length, deserializedSpeaker.Styles.Length);
    }
}