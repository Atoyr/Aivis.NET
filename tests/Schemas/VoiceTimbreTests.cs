using System.Text.Json;
using Xunit;

namespace Aivis.Schemas.Tests;

public class VoiceTimbreTests
{
    [Theory]
    [InlineData("YoungMale", VoiceTimbre.YoungMale)]
    [InlineData("YoungFemale", VoiceTimbre.YoungFemale)]
    [InlineData("YouthfulMale", VoiceTimbre.YouthfulMale)]
    [InlineData("YouthfulFemale", VoiceTimbre.YouthfulFemale)]
    [InlineData("AdultMale", VoiceTimbre.AdultMale)]
    [InlineData("AdultFemale", VoiceTimbre.AdultFemale)]
    [InlineData("MiddleAgedMale", VoiceTimbre.MiddleAgedMale)]
    [InlineData("MiddleAgedFemale", VoiceTimbre.MiddleAgedFemale)]
    [InlineData("ElderlyMale", VoiceTimbre.ElderlyMale)]
    [InlineData("ElderlyFemale", VoiceTimbre.ElderlyFemale)]
    [InlineData("Neutral", VoiceTimbre.Neutral)]
    [InlineData("Baby", VoiceTimbre.Baby)]
    [InlineData("Mechanical", VoiceTimbre.Mechanical)]
    [InlineData("Other", VoiceTimbre.Other)]
    public void Should_DeserializeFromString_Success(string jsonValue, VoiceTimbre expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<VoiceTimbre>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(VoiceTimbre.YoungMale, "YoungMale")]
    [InlineData(VoiceTimbre.YoungFemale, "YoungFemale")]
    [InlineData(VoiceTimbre.YouthfulMale, "YouthfulMale")]
    [InlineData(VoiceTimbre.YouthfulFemale, "YouthfulFemale")]
    [InlineData(VoiceTimbre.AdultMale, "AdultMale")]
    [InlineData(VoiceTimbre.AdultFemale, "AdultFemale")]
    [InlineData(VoiceTimbre.MiddleAgedMale, "MiddleAgedMale")]
    [InlineData(VoiceTimbre.MiddleAgedFemale, "MiddleAgedFemale")]
    [InlineData(VoiceTimbre.ElderlyMale, "ElderlyMale")]
    [InlineData(VoiceTimbre.ElderlyFemale, "ElderlyFemale")]
    [InlineData(VoiceTimbre.Neutral, "Neutral")]
    [InlineData(VoiceTimbre.Baby, "Baby")]
    [InlineData(VoiceTimbre.Mechanical, "Mechanical")]
    [InlineData(VoiceTimbre.Other, "Other")]
    public void Should_SerializeToString_Success(VoiceTimbre value, string expected)
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
        string json = "\"InvalidTimbre\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<VoiceTimbre>(json));
    }
}