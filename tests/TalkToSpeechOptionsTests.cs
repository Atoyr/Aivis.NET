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

    // StyleName プロパティのテスト
    [Fact]
    public void StyleName_DefaultValue_IsNormal()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal("ノーマル", options.StyleName);
    }

    [Fact]
    public void StyleName_SetValidName_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        options.StyleName = "上機嫌";

        Assert.Equal("上機嫌", options.StyleName);
        Assert.Equal(0, options.StyleId); // StyleIdはデフォルト値にリセットされる
    }

    [Fact]
    public void StyleName_SetToDefault_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        options.StyleName = "上機嫌";

        options.StyleName = "ノーマル";

        Assert.Equal("ノーマル", options.StyleName);
    }

    [Fact]
    public void StyleId_SetValue_ResetsStyleNameToDefault()
    {
        var options = new TalkToSpeechOptions();
        options.StyleName = "上機嫌";

        options.StyleId = 10;

        Assert.Equal(10, options.StyleId);
        Assert.Equal("ノーマル", options.StyleName);
    }

    [Fact]
    public void SetStyle_WithStyleName_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetStyle("上機嫌");

        Assert.Equal("上機嫌", options.StyleName);
        Assert.Equal(0, options.StyleId);
        Assert.Same(options, result);
    }

    // Language プロパティのテスト
    [Fact]
    public void Language_DefaultValue_IsJa()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal("ja", options.Language);
    }

    [Theory]
    [InlineData("ja")]
    [InlineData("ja-JP")]
    [InlineData("en")]
    public void Language_CanBeSet(string language)
    {
        var options = new TalkToSpeechOptions();

        options.Language = language;

        Assert.Equal(language, options.Language);
    }

    [Fact]
    public void SetLanguage_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetLanguage("ja-JP");

        Assert.Equal("ja-JP", options.Language);
        Assert.Same(options, result);
    }

    // SpeakingRate プロパティのテスト
    [Fact]
    public void SpeakingRate_DefaultValue_Is1()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(1.0, options.SpeakingRate);
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void SpeakingRate_ValidRange_SetsCorrectly(double rate)
    {
        var options = new TalkToSpeechOptions();

        options.SpeakingRate = rate;

        Assert.Equal(rate, options.SpeakingRate);
    }

    [Theory]
    [InlineData(0.4)]
    [InlineData(2.1)]
    [InlineData(-1.0)]
    public void SpeakingRate_InvalidRange_ThrowsArgumentOutOfRangeException(double rate)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SpeakingRate = rate);
    }

    [Fact]
    public void SetSpeakingRate_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetSpeakingRate(1.5);

        Assert.Equal(1.5, options.SpeakingRate);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetSpeakingRate_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetSpeakingRate(2.5));
    }

    // EmotionIntensity プロパティのテスト
    [Fact]
    public void EmotionIntensity_DefaultValue_Is1()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(1.0, options.EmotionIntensity);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void EmotionIntensity_ValidRange_SetsCorrectly(double intensity)
    {
        var options = new TalkToSpeechOptions();

        options.EmotionIntensity = intensity;

        Assert.Equal(intensity, options.EmotionIntensity);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(2.1)]
    [InlineData(-1.0)]
    public void EmotionIntensity_InvalidRange_ThrowsArgumentOutOfRangeException(double intensity)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.EmotionIntensity = intensity);
    }

    [Fact]
    public void SetEmotionIntensity_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetEmotionIntensity(1.5);

        Assert.Equal(1.5, options.EmotionIntensity);
        Assert.Same(options, result);
    }

    // TempoDynamics プロパティのテスト
    [Fact]
    public void TempoDynamics_DefaultValue_Is1()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(1.0, options.TempoDynamics);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void TempoDynamics_ValidRange_SetsCorrectly(double dynamics)
    {
        var options = new TalkToSpeechOptions();

        options.TempoDynamics = dynamics;

        Assert.Equal(dynamics, options.TempoDynamics);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(2.1)]
    public void TempoDynamics_InvalidRange_ThrowsArgumentOutOfRangeException(double dynamics)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.TempoDynamics = dynamics);
    }

    [Fact]
    public void SetTempoDynamics_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetTempoDynamics(1.5);

        Assert.Equal(1.5, options.TempoDynamics);
        Assert.Same(options, result);
    }

    // Pitch プロパティのテスト
    [Fact]
    public void Pitch_DefaultValue_Is0()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(0.0, options.Pitch);
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    public void Pitch_ValidRange_SetsCorrectly(double pitch)
    {
        var options = new TalkToSpeechOptions();

        options.Pitch = pitch;

        Assert.Equal(pitch, options.Pitch);
    }

    [Theory]
    [InlineData(-1.1)]
    [InlineData(1.1)]
    [InlineData(2.0)]
    public void Pitch_InvalidRange_ThrowsArgumentOutOfRangeException(double pitch)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.Pitch = pitch);
    }

    [Fact]
    public void SetPitch_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetPitch(0.5);

        Assert.Equal(0.5, options.Pitch);
        Assert.Same(options, result);
    }

    // Volume プロパティのテスト
    [Fact]
    public void Volume_DefaultValue_Is1()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(1.0, options.Volume);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(1.5)]
    [InlineData(2.0)]
    public void Volume_ValidRange_SetsCorrectly(double volume)
    {
        var options = new TalkToSpeechOptions();

        options.Volume = volume;

        Assert.Equal(volume, options.Volume);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(2.1)]
    public void Volume_InvalidRange_ThrowsArgumentOutOfRangeException(double volume)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.Volume = volume);
    }

    [Fact]
    public void SetVolume_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetVolume(1.5);

        Assert.Equal(1.5, options.Volume);
        Assert.Same(options, result);
    }

    // LeadingSilenceSeconds プロパティのテスト
    [Fact]
    public void LeadingSilenceSeconds_DefaultValue_Is01()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(0.1, options.LeadingSilenceSeconds);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.1)]
    [InlineData(30.0)]
    [InlineData(60.0)]
    public void LeadingSilenceSeconds_ValidRange_SetsCorrectly(double seconds)
    {
        var options = new TalkToSpeechOptions();

        options.LeadingSilenceSeconds = seconds;

        Assert.Equal(seconds, options.LeadingSilenceSeconds);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(60.1)]
    public void LeadingSilenceSeconds_InvalidRange_ThrowsArgumentOutOfRangeException(double seconds)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.LeadingSilenceSeconds = seconds);
    }

    [Fact]
    public void SetLeadingSilenceSeconds_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetLeadingSilenceSeconds(0.5);

        Assert.Equal(0.5, options.LeadingSilenceSeconds);
        Assert.Same(options, result);
    }

    // TrailingSilenceSeconds プロパティのテスト
    [Fact]
    public void TrailingSilenceSeconds_DefaultValue_Is01()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(0.1, options.TrailingSilenceSeconds);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.1)]
    [InlineData(30.0)]
    [InlineData(60.0)]
    public void TrailingSilenceSeconds_ValidRange_SetsCorrectly(double seconds)
    {
        var options = new TalkToSpeechOptions();

        options.TrailingSilenceSeconds = seconds;

        Assert.Equal(seconds, options.TrailingSilenceSeconds);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(60.1)]
    public void TrailingSilenceSeconds_InvalidRange_ThrowsArgumentOutOfRangeException(double seconds)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.TrailingSilenceSeconds = seconds);
    }

    [Fact]
    public void SetTrailingSilenceSeconds_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetTrailingSilenceSeconds(0.5);

        Assert.Equal(0.5, options.TrailingSilenceSeconds);
        Assert.Same(options, result);
    }

    // LineBreakSilenceSeconds プロパティのテスト
    [Fact]
    public void LineBreakSilenceSeconds_DefaultValue_Is04()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(0.4, options.LineBreakSilenceSeconds);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.4)]
    [InlineData(30.0)]
    [InlineData(60.0)]
    public void LineBreakSilenceSeconds_ValidRange_SetsCorrectly(double seconds)
    {
        var options = new TalkToSpeechOptions();

        options.LineBreakSilenceSeconds = seconds;

        Assert.Equal(seconds, options.LineBreakSilenceSeconds);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(60.1)]
    public void LineBreakSilenceSeconds_InvalidRange_ThrowsArgumentOutOfRangeException(double seconds)
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.LineBreakSilenceSeconds = seconds);
    }

    [Fact]
    public void SetLineBreakSilenceSeconds_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetLineBreakSilenceSeconds(0.5);

        Assert.Equal(0.5, options.LineBreakSilenceSeconds);
        Assert.Same(options, result);
    }

    // SetSilenceSeconds メソッドのテスト
    [Fact]
    public void SetSilenceSeconds_TwoParameters_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetSilenceSeconds(0.2, 0.3);

        Assert.Equal(0.2, options.LeadingSilenceSeconds);
        Assert.Equal(0.3, options.TrailingSilenceSeconds);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetSilenceSeconds_ThreeParameters_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetSilenceSeconds(0.2, 0.3, 0.5);

        Assert.Equal(0.2, options.LeadingSilenceSeconds);
        Assert.Equal(0.3, options.TrailingSilenceSeconds);
        Assert.Equal(0.5, options.LineBreakSilenceSeconds);
        Assert.Same(options, result);
    }

    // OutputFormat with string のテスト
    [Theory]
    [InlineData("wav", MediaType.WAV)]
    [InlineData("flac", MediaType.FLAC)]
    [InlineData("mp3", MediaType.MP3)]
    [InlineData("aac", MediaType.AAC)]
    [InlineData("opus", MediaType.OPUS)]
    [InlineData("WAV", MediaType.WAV)]  // 大文字小文字を区別しない
    [InlineData("MP3", MediaType.MP3)]
    public void SetOutputFormat_ValidStringFormat_SetsCorrectly(string format, MediaType expectedType)
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetOutputFormat(format);

        Assert.Equal(expectedType, options.OutputFormat);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetOutputFormat_InvalidStringFormat_ThrowsArgumentException()
    {
        var options = new TalkToSpeechOptions();

        var exception = Assert.Throws<ArgumentException>(() => options.SetOutputFormat("invalid"));
        Assert.Equal("format", exception.ParamName);
        Assert.Contains("Invalid format", exception.Message);
    }

    // OutputBitrate プロパティのテスト
    [Fact]
    public void OutputBitrate_DefaultValue_IsNull()
    {
        var options = new TalkToSpeechOptions();

        Assert.Null(options.OutputBitrate);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(128)]
    [InlineData(192)]
    [InlineData(320)]
    public void OutputBitrate_ValidRange_SetsCorrectly(int bitrate)
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.MP3; // WAV/FLAC以外に設定

        options.OutputBitrate = bitrate;

        Assert.Equal(bitrate, options.OutputBitrate);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(321)]
    public void OutputBitrate_InvalidRange_ThrowsArgumentOutOfRangeException(int bitrate)
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.MP3;

        Assert.Throws<ArgumentOutOfRangeException>(() => options.OutputBitrate = bitrate);
    }

    [Theory]
    [InlineData(MediaType.WAV)]
    [InlineData(MediaType.FLAC)]
    public void OutputBitrate_WavOrFlacFormat_ThrowsInvalidOperationException(MediaType format)
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = format;

        Assert.Throws<InvalidOperationException>(() => options.OutputBitrate = 128);
    }

    [Fact]
    public void OutputBitrate_SetToNull_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.MP3;
        options.OutputBitrate = 128;

        options.OutputBitrate = null;

        Assert.Null(options.OutputBitrate);
    }

    [Fact]
    public void SetOutputBitrate_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.MP3;

        var result = options.SetOutputBitrate(192);

        Assert.Equal(192, options.OutputBitrate);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetOutputBitrate_Null_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetOutputBitrate(null);

        Assert.Null(options.OutputBitrate);
        Assert.Same(options, result);
    }

    // OutputSamplingRate プロパティのテスト
    [Fact]
    public void OutputSamplingRate_DefaultValue_Is44100()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(44100, options.OutputSamplingRate);
    }

    [Theory]
    [InlineData(22050)]
    [InlineData(44100)]
    [InlineData(48000)]
    public void OutputSamplingRate_NonOpusFormat_SetsCorrectly(int rate)
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.MP3;

        options.OutputSamplingRate = rate;

        Assert.Equal(rate, options.OutputSamplingRate);
    }

    [Theory]
    [InlineData(8000, 8000)]
    [InlineData(12000, 12000)]
    [InlineData(16000, 16000)]
    [InlineData(24000, 24000)]
    [InlineData(48000, 48000)]
    public void OutputSamplingRate_OpusFormat_SupportedRates_SetsCorrectly(int inputRate, int expectedRate)
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.OPUS;

        options.OutputSamplingRate = inputRate;

        Assert.Equal(expectedRate, options.OutputSamplingRate);
    }

    [Theory]
    [InlineData(7999, 8000)]   // 8000未満は8000に調整
    [InlineData(10000, 12000)] // 8000-12000の間は12000に調整
    [InlineData(14000, 16000)] // 12000-16000の間は16000に調整
    [InlineData(20000, 24000)] // 16000-24000の間は24000に調整
    [InlineData(30000, 48000)] // 24000-48000の間は48000に調整
    [InlineData(60000, 48000)] // 48000超過は48000に調整
    public void OutputSamplingRate_OpusFormat_UnsupportedRates_AdjustsToNearestSupported(int inputRate, int expectedRate)
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.OPUS;

        options.OutputSamplingRate = inputRate;

        Assert.Equal(expectedRate, options.OutputSamplingRate);
    }

    [Fact]
    public void SetOutputSamplingRate_ValidValue_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetOutputSamplingRate(48000);

        Assert.Equal(48000, options.OutputSamplingRate);
        Assert.Same(options, result);
    }

    // OutputAudioChannels プロパティのテスト
    [Fact]
    public void OutputAudioChannels_DefaultValue_IsMono()
    {
        var options = new TalkToSpeechOptions();

        Assert.Equal(AudioChannel.Mono, options.OutputAudioChannels);
    }

    [Theory]
    [InlineData(AudioChannel.Mono)]
    [InlineData(AudioChannel.Stereo)]
    public void OutputAudioChannels_ValidChannel_SetsCorrectly(AudioChannel channel)
    {
        var options = new TalkToSpeechOptions();

        options.OutputAudioChannels = channel;

        Assert.Equal(channel, options.OutputAudioChannels);
    }

    [Fact]
    public void SetOutputAudioChannels_WithEnum_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetOutputAudioChannels(AudioChannel.Stereo);

        Assert.Equal(AudioChannel.Stereo, options.OutputAudioChannels);
        Assert.Same(options, result);
    }

    [Theory]
    [InlineData("mono", AudioChannel.Mono)]
    [InlineData("stereo", AudioChannel.Stereo)]
    [InlineData("MONO", AudioChannel.Mono)]   // 大文字小文字を区別しない
    [InlineData("STEREO", AudioChannel.Stereo)]
    public void SetOutputAudioChannels_WithValidString_SetsCorrectly(string channelString, AudioChannel expectedChannel)
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetOutputAudioChannels(channelString);

        Assert.Equal(expectedChannel, options.OutputAudioChannels);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetOutputAudioChannels_WithInvalidString_ThrowsArgumentException()
    {
        var options = new TalkToSpeechOptions();

        var exception = Assert.Throws<ArgumentException>(() => options.SetOutputAudioChannels("invalid"));
        Assert.Equal("outputAudioChannels", exception.ParamName);
        Assert.Contains("Invalid outputAudioChannels", exception.Message);
    }

    // 複合的なFluent APIのテスト
    [Fact]
    public void FluentInterface_CompleteMethodChaining_WorksCorrectly()
    {
        var options = new TalkToSpeechOptions();
        var speakerUuid = Guid.NewGuid();
        var userDictionaryUuid = Guid.NewGuid();

        var result = options
            .SetSpeakerUuid(speakerUuid)
            .SetStyle("上機嫌")
            .SetUserDictionary(userDictionaryUuid)
            .SetUseSsml(false)
            .SetLanguage("ja-JP")
            .SetSpeakingRate(1.5)
            .SetEmotionIntensity(1.2)
            .SetTempoDynamics(1.3)
            .SetPitch(0.1)
            .SetVolume(1.1)
            .SetSilenceSeconds(0.2, 0.3, 0.5)
            .SetOutputFormat("opus")
            .SetOutputBitrate(128)
            .SetOutputSamplingRate(48000)
            .SetOutputAudioChannels("stereo");

        Assert.Equal(speakerUuid, options.SpeakerUuid);
        Assert.Equal("上機嫌", options.StyleName);
        Assert.Equal(0, options.StyleId);
        Assert.Equal(userDictionaryUuid, options.UserDictionaryUuid);
        Assert.False(options.UseSsml);
        Assert.Equal("ja-JP", options.Language);
        Assert.Equal(1.5, options.SpeakingRate);
        Assert.Equal(1.2, options.EmotionIntensity);
        Assert.Equal(1.3, options.TempoDynamics);
        Assert.Equal(0.1, options.Pitch);
        Assert.Equal(1.1, options.Volume);
        Assert.Equal(0.2, options.LeadingSilenceSeconds);
        Assert.Equal(0.3, options.TrailingSilenceSeconds);
        Assert.Equal(0.5, options.LineBreakSilenceSeconds);
        Assert.Equal(MediaType.OPUS, options.OutputFormat);
        Assert.Equal(128, options.OutputBitrate);
        Assert.Equal(48000, options.OutputSamplingRate);
        Assert.Equal(AudioChannel.Stereo, options.OutputAudioChannels);
        Assert.Same(options, result);
    }

    // StyleId と StyleName の相互排他動作のテスト
    [Fact]
    public void StyleIdAndStyleName_MutuallyExclusive_StyleIdSetsToDefault()
    {
        var options = new TalkToSpeechOptions();

        // StyleNameを設定してからStyleIdを設定
        options.StyleName = "悲しみ";
        options.StyleId = 15;

        Assert.Equal(15, options.StyleId);
        Assert.Equal("ノーマル", options.StyleName); // StyleNameはデフォルトにリセット
    }

    [Fact]
    public void StyleIdAndStyleName_MutuallyExclusive_StyleNameSetsToDefault()
    {
        var options = new TalkToSpeechOptions();

        // StyleIdを設定してからStyleNameを設定
        options.StyleId = 20;
        options.StyleName = "怒り";

        Assert.Equal(0, options.StyleId); // StyleIdはデフォルトにリセット
        Assert.Equal("怒り", options.StyleName);
    }

    // エッジケースと境界値のテスト
    [Theory]
    [InlineData(0.5)]  // 最小値
    [InlineData(2.0)]  // 最大値
    public void SpeakingRate_BoundaryValues_SetsCorrectly(double rate)
    {
        var options = new TalkToSpeechOptions();

        options.SpeakingRate = rate;

        Assert.Equal(rate, options.SpeakingRate);
    }

    [Theory]
    [InlineData(0.0)]  // 最小値
    [InlineData(2.0)]  // 最大値
    public void EmotionIntensity_BoundaryValues_SetsCorrectly(double intensity)
    {
        var options = new TalkToSpeechOptions();

        options.EmotionIntensity = intensity;

        Assert.Equal(intensity, options.EmotionIntensity);
    }

    [Theory]
    [InlineData(0.0)]  // 最小値
    [InlineData(2.0)]  // 最大値
    public void TempoDynamics_BoundaryValues_SetsCorrectly(double dynamics)
    {
        var options = new TalkToSpeechOptions();

        options.TempoDynamics = dynamics;

        Assert.Equal(dynamics, options.TempoDynamics);
    }

    [Theory]
    [InlineData(-1.0)]  // 最小値
    [InlineData(1.0)]   // 最大値
    public void Pitch_BoundaryValues_SetsCorrectly(double pitch)
    {
        var options = new TalkToSpeechOptions();

        options.Pitch = pitch;

        Assert.Equal(pitch, options.Pitch);
    }

    [Theory]
    [InlineData(0.0)]  // 最小値
    [InlineData(2.0)]  // 最大値
    public void Volume_BoundaryValues_SetsCorrectly(double volume)
    {
        var options = new TalkToSpeechOptions();

        options.Volume = volume;

        Assert.Equal(volume, options.Volume);
    }

    [Theory]
    [InlineData(0.0)]   // 最小値
    [InlineData(60.0)]  // 最大値
    public void SilenceSeconds_BoundaryValues_SetsCorrectly(double seconds)
    {
        var options = new TalkToSpeechOptions();

        options.LeadingSilenceSeconds = seconds;
        options.TrailingSilenceSeconds = seconds;
        options.LineBreakSilenceSeconds = seconds;

        Assert.Equal(seconds, options.LeadingSilenceSeconds);
        Assert.Equal(seconds, options.TrailingSilenceSeconds);
        Assert.Equal(seconds, options.LineBreakSilenceSeconds);
    }

    // OutputBitrateのフォーマット依存テスト
    [Fact]
    public void OutputBitrate_FormatChangeFromWAVToMP3_AllowsSettingBitrate()
    {
        var options = new TalkToSpeechOptions();

        // 最初にWAV形式に設定（ビットレート設定不可）
        options.OutputFormat = MediaType.WAV;

        // MP3に変更してからビットレートを設定
        options.OutputFormat = MediaType.MP3;
        options.OutputBitrate = 192;

        Assert.Equal(192, options.OutputBitrate);
    }

    [Fact]
    public void OutputBitrate_FormatChangeFromMP3ToWAV_KeepsBitrateButCannotChange()
    {
        var options = new TalkToSpeechOptions();

        // MP3形式でビットレートを設定
        options.OutputFormat = MediaType.MP3;
        options.OutputBitrate = 192;

        // WAV形式に変更
        options.OutputFormat = MediaType.WAV;

        // 既存のビットレート値は保持されるが、新しい値は設定できない
        Assert.Throws<InvalidOperationException>(() => options.OutputBitrate = 256);
    }

    // OutputSamplingRateのOpus特別処理テスト
    [Fact]
    public void OutputSamplingRate_FormatChangeToOpus_AdjustsExistingRate()
    {
        var options = new TalkToSpeechOptions();

        // 非Opus形式で任意のサンプリングレートを設定
        options.OutputFormat = MediaType.MP3;
        options.OutputSamplingRate = 22050;

        // Opus形式に変更
        options.OutputFormat = MediaType.OPUS;

        // 22050は対応していないので24000に自動調整される
        Assert.Equal(24000, options.OutputSamplingRate);
    }

    [Fact]
    public void OutputSamplingRate_OpusFormatMinimumValue_AdjustsToSupported()
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.OPUS;

        // 1000Hzのような極端に低い値
        options.OutputSamplingRate = 1000;

        // 8000Hzに調整される
        Assert.Equal(8000, options.OutputSamplingRate);
    }

    // Setterメソッドのエラーハンドリングテスト
    [Fact]
    public void SetEmotionIntensity_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetEmotionIntensity(2.5));
    }

    [Fact]
    public void SetTempoDynamics_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetTempoDynamics(-0.5));
    }

    [Fact]
    public void SetPitch_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetPitch(1.5));
    }

    [Fact]
    public void SetVolume_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetVolume(3.0));
    }

    [Fact]
    public void SetLeadingSilenceSeconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetLeadingSilenceSeconds(70.0));
    }

    [Fact]
    public void SetTrailingSilenceSeconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetTrailingSilenceSeconds(-1.0));
    }

    [Fact]
    public void SetLineBreakSilenceSeconds_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetLineBreakSilenceSeconds(100.0));
    }

    [Fact]
    public void SetOutputBitrate_InvalidValue_ThrowsArgumentOutOfRangeException()
    {
        var options = new TalkToSpeechOptions();
        options.OutputFormat = MediaType.MP3;

        Assert.Throws<ArgumentOutOfRangeException>(() => options.SetOutputBitrate(500));
    }

    // null や空文字列のテスト
    [Fact]
    public void SetLanguage_EmptyString_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetLanguage("");

        Assert.Equal("", options.Language);
        Assert.Same(options, result);
    }

    [Fact]
    public void SetStyle_EmptyStringStyleName_SetsCorrectly()
    {
        var options = new TalkToSpeechOptions();

        var result = options.SetStyle("");

        Assert.Equal("", options.StyleName);
        Assert.Equal(0, options.StyleId);
        Assert.Same(options, result);
    }

    // デフォルトコンストラクタの詳細テスト
    [Fact]
    public void Constructor_AllDefaultValues_AreCorrect()
    {
        var options = new TalkToSpeechOptions();

        // 基本的なプロパティ
        Assert.Null(options.SpeakerUuid);
        Assert.Equal(0, options.StyleId);
        Assert.Equal("ノーマル", options.StyleName);
        Assert.Null(options.UserDictionaryUuid);
        Assert.True(options.UseSsml);
        Assert.Equal("ja", options.Language);

        // 音声パラメータ
        Assert.Equal(1.0, options.SpeakingRate);
        Assert.Equal(1.0, options.EmotionIntensity);
        Assert.Equal(1.0, options.TempoDynamics);
        Assert.Equal(0.0, options.Pitch);
        Assert.Equal(1.0, options.Volume);

        // 無音区間
        Assert.Equal(0.1, options.LeadingSilenceSeconds);
        Assert.Equal(0.1, options.TrailingSilenceSeconds);
        Assert.Equal(0.4, options.LineBreakSilenceSeconds);

        // 出力形式
        Assert.Equal(MediaType.MP3, options.OutputFormat);
        Assert.Null(options.OutputBitrate);
        Assert.Equal(44100, options.OutputSamplingRate);
        Assert.Equal(AudioChannel.Mono, options.OutputAudioChannels);
    }
}