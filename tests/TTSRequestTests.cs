using Aivis.Schemas;

namespace Aivis.Tests;

public class TTSRequestTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        var modelUuid = "550e8400-e29b-41d4-a716-446655440000";
        var text = "テストテキスト";

        var request = new TTSRequest(modelUuid, text);

        Assert.Equal(modelUuid, request.ModelUuid);
        Assert.Equal(text, request.Text);
        Assert.Equal("mp3", request.OutputFormat);
        Assert.True(request.UseSsml);
        Assert.Equal(0, request.StyleId);
    }

    [Fact]
    public void ModelUuid_ValidGuid_SetsCorrectly()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");
        var newUuid = "6ba7b810-9dad-11d1-80b4-00c04fd430c8";

        request.ModelUuid = newUuid;

        Assert.Equal(newUuid, request.ModelUuid);
    }

    [Fact]
    public void ModelUuid_InvalidGuid_ThrowsArgumentException()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        Assert.Throws<ArgumentException>(() => request.ModelUuid = "invalid-uuid");
    }

    [Fact]
    public void SpeakerUuid_ValidGuid_SetsCorrectly()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");
        var speakerUuid = "6ba7b810-9dad-11d1-80b4-00c04fd430c8";

        request.SpeakerUuid = speakerUuid;

        Assert.Equal(speakerUuid, request.SpeakerUuid);
    }

    [Fact]
    public void SpeakerUuid_Null_SetsToNull()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        request.SpeakerUuid = null;

        Assert.Null(request.SpeakerUuid);
    }

    [Fact]
    public void SpeakerUuid_InvalidGuid_ThrowsArgumentException()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        Assert.Throws<ArgumentException>(() => request.SpeakerUuid = "invalid-uuid");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(31)]
    public void StyleId_ValidRange_SetsCorrectly(int styleId)
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        request.StyleId = styleId;

        Assert.Equal(styleId, request.StyleId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    [InlineData(100)]
    public void StyleId_InvalidRange_ThrowsArgumentOutOfRangeException(int styleId)
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        Assert.Throws<ArgumentOutOfRangeException>(() => request.StyleId = styleId);
    }

    [Fact]
    public void UserDictionaryUuid_ValidGuid_SetsCorrectly()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");
        var dictionaryUuid = "6ba7b810-9dad-11d1-80b4-00c04fd430c8";

        request.UserDictionaryUuid = dictionaryUuid;

        Assert.Equal(dictionaryUuid, request.UserDictionaryUuid);
    }

    [Fact]
    public void UserDictionaryUuid_Null_SetsToNull()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        request.UserDictionaryUuid = null;

        Assert.Null(request.UserDictionaryUuid);
    }

    [Fact]
    public void UserDictionaryUuid_InvalidGuid_ThrowsArgumentException()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        Assert.Throws<ArgumentException>(() => request.UserDictionaryUuid = "invalid-uuid");
    }

    [Fact]
    public void Text_CanBeSet()
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "original");
        var newText = "新しいテキスト";

        request.Text = newText;

        Assert.Equal(newText, request.Text);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UseSsml_CanBeSet(bool useSsml)
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        request.UseSsml = useSsml;

        Assert.Equal(useSsml, request.UseSsml);
    }

    [Theory]
    [InlineData("wav")]
    [InlineData("flac")]
    [InlineData("mp3")]
    [InlineData("aac")]
    [InlineData("opus")]
    public void OutputFormat_CanBeSet(string format)
    {
        var request = new TTSRequest("550e8400-e29b-41d4-a716-446655440000", "test");

        request.OutputFormat = format;

        Assert.Equal(format, request.OutputFormat);
    }
}