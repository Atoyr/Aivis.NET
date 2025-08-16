namespace Aivis.Tests;

public class TalkToSpeechOptionsTests
{
    [Fact]
    public void Constructor_DefaultValues_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        Assert.Null(options.SpeakerUuid);
        Assert.Equal(0, options.StyleId);
        Assert.Null(options.UserDictionaryUuid);
        Assert.True(options.UseSsml);
        Assert.Equal(MediaType.MP3, options.OutputFormat);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(31)]
    public void StyleId_ValidRange_SetsCorrectly(int styleId)
    {
        var options = new TalkToSpeechOptions();

        options.StyleId = styleId;

        Assert.Equal(styleId, options.StyleId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    [InlineData(100)]
    public void StyleId_InvalidRange_ThrowsArgumentOutOfRangeException(int styleId)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.StyleId = styleId);
    }

    [Fact]
    public void SpeakerUuid_ValidGuid_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var uuid = Guid.NewGuid();

        options.SpeakerUuid = uuid;

        Assert.Equal(uuid, options.SpeakerUuid);
    }

    [Fact]
    public void SpeakerUuid_Null_SetsToNull()
    {
        var options = new TalkToSpeechOptions();
        options.SpeakerUuid = Guid.NewGuid();

        options.SpeakerUuid = null;

        Assert.Null(options.SpeakerUuid);
    }

    [Fact]
    public void UserDictionaryUuid_ValidGuid_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var uuid = Guid.NewGuid();

        options.UserDictionaryUuid = uuid;

        Assert.Equal(uuid, options.UserDictionaryUuid);
    }

    [Fact]
    public void UserDictionaryUuid_Null_SetsToNull()
    {
        var options = new TalkToSpeechOptions();
        options.UserDictionaryUuid = Guid.NewGuid();

        options.UserDictionaryUuid = null;

        Assert.Null(options.UserDictionaryUuid);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UseSsml_CanBeSet(bool useSsml)
    {
        var options = new TalkToSpeechOptions();

        options.UseSsml = useSsml;

        Assert.Equal(useSsml, options.UseSsml);
    }

    [Theory]
    [InlineData(MediaType.WAV)]
    [InlineData(MediaType.FLAC)]
    [InlineData(MediaType.MP3)]
    [InlineData(MediaType.AAC)]
    [InlineData(MediaType.OPUS)]
    public void OutputFormat_CanBeSet(MediaType format)
    {
        var options = new TalkToSpeechOptions();

        options.OutputFormat = format;

        Assert.Equal(format, options.OutputFormat);
    }

    [Fact]
    public void SetSpeakerUuid_ValidGuidString_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var uuid = Guid.NewGuid();
        var uuidString = uuid.ToString();

        var result = options.SetSpeakerUuid(uuidString);

        Assert.Equal(uuid, options.SpeakerUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetSpeakerUuid_ValidGuid_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var uuid = Guid.NewGuid();

        var result = options.SetSpeakerUuid(uuid);

        Assert.Equal(uuid, options.SpeakerUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetSpeakerUuid_NullString_SetsToNull()
    {
        var options = new TalkToSpeechOptions();
        options.SpeakerUuid = Guid.NewGuid();

        var result = options.SetSpeakerUuid((string?)null);

        Assert.Null(options.SpeakerUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetSpeakerUuid_NullGuid_SetsToNull()
    {
        var options = new TalkToSpeechOptions();
        options.SpeakerUuid = Guid.NewGuid();

        var result = options.SetSpeakerUuid((Guid?)null);

        Assert.Null(options.SpeakerUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetSpeakerUuid_InvalidGuidString_ThrowsArgumentException()
    {
        var options = new TalkToSpeechOptions();

        var exception = Assert.Throws<ArgumentException>(() => options.SetSpeakerUuid("invalid-uuid"));
        Assert.Equal("speakerUuid", exception.ParamName);
        Assert.Contains("Invalid UUID format", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(15)]
    [InlineData(31)]
    public void SetStyle_ValidRange_SetsCorrectly(int styleId)
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetStyle(styleId);

        Assert.Equal(styleId, options.StyleId);
        Assert.Same(options, result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    [InlineData(100)]
    public void SetStyle_InvalidRange_ThrowsArgumentOutOfRangeException(int styleId)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetStyle(styleId));
    }

    [Fact]
    public void SetUserDictionary_ValidGuidString_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var uuid = Guid.NewGuid();
        var uuidString = uuid.ToString();

        var result = options.SetUserDictionary(uuidString);

        Assert.Equal(uuid, options.UserDictionaryUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetUserDictionary_ValidGuid_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var uuid = Guid.NewGuid();

        var result = options.SetUserDictionary(uuid);

        Assert.Equal(uuid, options.UserDictionaryUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetUserDictionary_NullString_SetsToNull()
    {
        var options = new TalkToSpeechOptions();
        options.UserDictionaryUuid = Guid.NewGuid();

        var result = options.SetUserDictionary((string?)null);

        Assert.Null(options.UserDictionaryUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetUserDictionary_NullGuid_SetsToNull()
    {
        var options = new TalkToSpeechOptions();
        options.UserDictionaryUuid = Guid.NewGuid();

        var result = options.SetUserDictionary((Guid?)null);

        Assert.Null(options.UserDictionaryUuid);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetUserDictionary_InvalidGuidString_ThrowsArgumentException()
    {
        var options = new TalkToSpeechOptions();

        var exception = Assert.Throws<ArgumentException>(() => options.SetUserDictionary("invalid-uuid"));
        Assert.Equal("userDictionaryUuid", exception.ParamName);
        Assert.Contains("Invalid UUID format", exception.Message);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetUseSsml_CanBeSet(bool useSsml)
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetUseSsml(useSsml);

        Assert.Equal(useSsml, options.UseSsml);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetUseSsml_DefaultParameter_SetsToTrue()
    {
        var options = new TalkToSpeechOptions();
        options.UseSsml = false;

        var result = options.SetUseSsml();

        Assert.True(options.UseSsml);
        Assert.Same(options, result);
    }

    [Theory]
    [InlineData(MediaType.WAV)]
    [InlineData(MediaType.FLAC)]
    [InlineData(MediaType.MP3)]
    [InlineData(MediaType.AAC)]
    [InlineData(MediaType.OPUS)]
    public void SetOutputFormat_CanBeSet(MediaType format)
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetOutputFormat(format);

        Assert.Equal(format, options.OutputFormat);
        Assert.Same(options, result);
    }

    [Fact]
    public void FluentInterface_MethodChaining_WorksCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var speakerUuid = Guid.NewGuid();
        var userDictionaryUuid = Guid.NewGuid();

        var result = options
            .SetSpeakerUuid(speakerUuid)
            .SetStyle(10)
            .SetUserDictionary(userDictionaryUuid)
            .SetUseSsml(false)
            .SetOutputFormat(MediaType.WAV);

        Assert.Equal(speakerUuid, options.SpeakerUuid);
        Assert.Equal(10, options.StyleId);
        Assert.Equal(userDictionaryUuid, options.UserDictionaryUuid);
        Assert.False(options.UseSsml);
        Assert.Equal(MediaType.WAV, options.OutputFormat);
        Assert.Same(options, result);
    }
}