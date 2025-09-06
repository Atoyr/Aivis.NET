using System.Text.Json;

using Aivis.Schemas;

namespace Aivis.Tests;

public class TTSRequestJsonBoundaryTests
{
    private const string ValidModel = "550e8400-e29b-41d4-a716-446655440000";

    [Fact]
    public void SpeakingRate_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t");
        req.SpeakingRate = 0.5;
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"speaking_rate\":0.5", jsonMin);

        req.SpeakingRate = 2.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"speaking_rate\":2", jsonMax);
    }

    [Fact]
    public void EmotionIntensity_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { EmotionIntensity = 0.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"emotion_intensity\":0", jsonMin);

        req.EmotionIntensity = 2.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"emotion_intensity\":2", jsonMax);
    }

    [Fact]
    public void TempoDynamics_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { TempoDynamics = 0.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"tempo_dynamics\":0", jsonMin);

        req.TempoDynamics = 2.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"tempo_dynamics\":2", jsonMax);
    }

    [Fact]
    public void Pitch_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { Pitch = -1.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"pitch\":-1", jsonMin);

        req.Pitch = 1.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"pitch\":1", jsonMax);
    }

    [Fact]
    public void Volume_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { Volume = 0.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"volume\":0", jsonMin);

        req.Volume = 2.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"volume\":2", jsonMax);
    }

    [Fact]
    public void LeadingSilenceSeconds_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { LeadingSilenceSeconds = 0.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"leading_silence_seconds\":0", jsonMin);

        req.LeadingSilenceSeconds = 60.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"leading_silence_seconds\":60", jsonMax);
    }

    [Fact]
    public void TrailingSilenceSeconds_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { TrailingSilenceSeconds = 0.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"trailing_silence_seconds\":0", jsonMin);

        req.TrailingSilenceSeconds = 60.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"trailing_silence_seconds\":60", jsonMax);
    }

    [Fact]
    public void LineBreakSilenceSeconds_Boundaries_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t") { LineBreakSilenceSeconds = 0.0 };
        var jsonMin = JsonSerializer.Serialize(req);
        Assert.Contains("\"line_break_silence_seconds\":0", jsonMin);

        req.LineBreakSilenceSeconds = 60.0;
        var jsonMax = JsonSerializer.Serialize(req);
        Assert.Contains("\"line_break_silence_seconds\":60", jsonMax);
    }

    [Fact]
    public void OutputSamplingRate_NonDefault_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t");
        var jsonDefault = JsonSerializer.Serialize(req);
        Assert.DoesNotContain("output_sampling_rate", jsonDefault);

        req.OutputSamplingRate = 48000;
        var json = JsonSerializer.Serialize(req);
        Assert.Contains("\"output_sampling_rate\":48000", json);
    }

    [Fact]
    public void OutputFormat_NonDefault_Serialized()
    {
        var req = new TTSRequest(ValidModel, "t");
        var jsonDefault = JsonSerializer.Serialize(req);
        Assert.DoesNotContain("output_format", jsonDefault);

        req.OutputFormat = "aac";
        var json = JsonSerializer.Serialize(req);
        Assert.Contains("\"output_format\":\"aac\"", json);
    }

    [Fact]
    public void OutputAudioChannels_DefaultOmitted_And_NonDefaultSerialized()
    {
        var req = new TTSRequest(ValidModel, "t");
        var jsonDefault = JsonSerializer.Serialize(req);
        Assert.DoesNotContain("output_audio_channels", jsonDefault);

        req.OutputAudioChannels = "stereo";
        var json = JsonSerializer.Serialize(req);
        Assert.Contains("\"output_audio_channels\":\"stereo\"", json);
    }
}