using System.Text.Json;

using Aivis.Schemas;

namespace Aivis.Tests;

public class TTSRequestAdvancedTests
{
    private const string ValidModel = "550e8400-e29b-41d4-a716-446655440000";

    [Fact]
    public void StyleId_SetNonDefault_ResetsStyleNameToDefault_AndSerializesStyleIdOnly()
    {
        var req = new TTSRequest(ValidModel, "text");

        req.StyleId = 5; // 非デフォルト
        var json = JsonSerializer.Serialize(req);

        Assert.Equal(5, req.StyleId);
        Assert.Equal(TTSRequest.DEFAULT_STYLE_NAME, req.StyleName);
        Assert.Contains("\"style_id\":5", json);
        Assert.DoesNotContain("style_name", json);
    }

    [Fact]
    public void StyleName_SetNonDefault_ResetsStyleIdToDefault_AndSerializesStyleNameOnly()
    {
        var req = new TTSRequest(ValidModel, "text");

        req.StyleName = "Energetic"; // 非デフォルト（ASCII にしてエスケープ差異を回避）
        var json = JsonSerializer.Serialize(req);

        Assert.Equal(TTSRequest.DEFAULT_STYLE_ID, req.StyleId);
        Assert.Equal("Energetic", req.StyleName);
        Assert.Contains("\"style_name\":\"Energetic\"", json);
        Assert.DoesNotContain("style_id", json);
    }

    [Fact]
    public void Language_Default_NotSerialized_And_Custom_SerializesWithTypoKey()
    {
        var req = new TTSRequest(ValidModel, "text");
        var jsonDefault = JsonSerializer.Serialize(req);
        Assert.DoesNotContain("language", jsonDefault); // デフォルトは非出力

        req.Language = "en";
        var jsonCustom = JsonSerializer.Serialize(req);
        Assert.Contains("\"language\":\"en\"", jsonCustom); // モデルのプロパティ名に合わせる
    }

    [Theory]
    [InlineData(0.49)]
    [InlineData(2.01)]
    public void SpeakingRate_OutOfRange_Throws(double rate)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.SpeakingRate = rate);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(2.01)]
    public void EmotionIntensity_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.EmotionIntensity = v);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(2.01)]
    public void TempoDynamics_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.TempoDynamics = v);
    }

    [Theory]
    [InlineData(-1.01)]
    [InlineData(1.01)]
    public void Pitch_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.Pitch = v);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(2.01)]
    public void Volume_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.Volume = v);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(60.01)]
    public void LeadingSilenceSeconds_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.LeadingSilenceSeconds = v);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(60.01)]
    public void TrailingSilenceSeconds_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.TrailingSilenceSeconds = v);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(60.01)]
    public void LineBreakSilenceSeconds_OutOfRange_Throws(double v)
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentOutOfRangeException>(() => req.LineBreakSilenceSeconds = v);
    }

    [Fact]
    public void OutputBitrate_OutOfRange_Throws()
    {
        var req = new TTSRequest(ValidModel, "text") { OutputFormat = "mp3" };
        Assert.Throws<ArgumentOutOfRangeException>(() => req.OutputBitrate = 7);
        Assert.Throws<ArgumentOutOfRangeException>(() => req.OutputBitrate = 321);
    }

    [Fact]
    public void OutputBitrate_DisallowedForWavFlac_Throws()
    {
        var req = new TTSRequest(ValidModel, "text") { OutputFormat = "wav" };
        Assert.Throws<InvalidOperationException>(() => req.OutputBitrate = 192);

        req.OutputFormat = "flac";
        Assert.Throws<InvalidOperationException>(() => req.OutputBitrate = 128);
    }

    [Fact]
    public void ChangingFormatToWavOrFlac_ClearsExistingBitrate()
    {
        var req = new TTSRequest(ValidModel, "text") { OutputFormat = "mp3", OutputBitrate = 192 };
        Assert.Equal(192, req.OutputBitrate);

        req.OutputFormat = "wav"; // 変更時にビットレートをクリア
        Assert.Null(req.OutputBitrate);
    }

    [Fact]
    public void Opus_OutputSamplingRate_AdjustsToNearestSupported()
    {
        var req = new TTSRequest(ValidModel, "text") { OutputFormat = "opus" };

        req.OutputSamplingRate = 22050; // -> 24000 に調整
        Assert.Equal(24000, req.OutputSamplingRate);

        req.OutputSamplingRate = 5000; // -> 8000 に調整
        Assert.Equal(8000, req.OutputSamplingRate);

        req.OutputSamplingRate = 96000; // -> 48000 に調整（上限）
        Assert.Equal(48000, req.OutputSamplingRate);
    }

    [Fact]
    public void OutputAudioChannels_ValidValues_SetAndSerialize()
    {
        var req = new TTSRequest(ValidModel, "text");

        req.OutputAudioChannels = "stereo";
        Assert.Equal("stereo", req.OutputAudioChannels);
        var json = JsonSerializer.Serialize(req);
        Assert.Contains("\"output_audio_channels\":\"stereo\"", json);

        req.OutputAudioChannels = "mono";
        Assert.Equal("mono", req.OutputAudioChannels);
    }

    [Fact]
    public void OutputAudioChannels_Invalid_Throws()
    {
        var req = new TTSRequest(ValidModel, "text");
        Assert.Throws<ArgumentException>(() => req.OutputAudioChannels = "quad");
    }

    [Fact]
    public void Defaults_NotSerializedInMinimalJson()
    {
        var req = new TTSRequest(ValidModel, "hello");
        var json = JsonSerializer.Serialize(req);

        // デフォルト値は省略される（Raw プロパティの条件）
        Assert.DoesNotContain("style_id", json);
        Assert.DoesNotContain("style_name", json);
        Assert.DoesNotContain("language", json);
        Assert.DoesNotContain("speaking_rate", json);
        Assert.DoesNotContain("emotion_intensity", json);
        Assert.DoesNotContain("tempo_dynamics", json);
        Assert.DoesNotContain("pitch", json);
        Assert.DoesNotContain("volume", json);
        Assert.DoesNotContain("leading_silence_seconds", json);
        Assert.DoesNotContain("trailing_silence_seconds", json);
        Assert.DoesNotContain("line_break_silence_seconds", json);
        Assert.DoesNotContain("output_sampling_rate", json);
        Assert.DoesNotContain("output_audio_channels", json);

        // 必須/常時: model_uuid, text, use_ssml
        Assert.Contains("model_uuid", json);
        Assert.Contains("text", json);
        Assert.Contains("use_ssml", json);

        // 既定の output_format は省略（非デフォルトのみ出力）
        Assert.DoesNotContain("output_format", json);
    }
}