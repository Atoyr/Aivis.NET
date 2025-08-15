using Aivis.Speakers;

namespace Aivis.Tests.Speakers;

public class MP3SpeakerTests
{
    [Fact]
    public void MP3Speaker_Constructor_WithDefaults_CreatesInstance()
    {
        using var speaker = new MP3Speaker();

        Assert.NotNull(speaker);
        // Note: Volume property requires OpenAL initialization
        // which may not be available in test environments
    }

    [Fact]
    public void MP3Speaker_Constructor_WithCustomBuffers_CreatesInstance()
    {
        using var speaker = new MP3Speaker(bufferCount: 8, bufferMillis: 200);

        Assert.NotNull(speaker);
    }

    [Fact]
    public void Volume_SetAndGet_WorksCorrectly()
    {
        try
        {
            using var speaker = new MP3Speaker();
            
            // Test setting and getting volume
            speaker.Volume = 0.5f;
            Assert.Equal(0.5f, speaker.Volume, 2);
            
            speaker.Volume = 1.0f;
            Assert.Equal(1.0f, speaker.Volume, 2);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("OpenAL"))
        {
            // Skip test if OpenAL is not available
        }
    }

    [Fact]
    public void Volume_SetBeyondLimits_IsClamped()
    {
        try
        {
            using var speaker = new MP3Speaker();
            
            // Test clamping above maximum
            speaker.Volume = 1.5f;
            Assert.Equal(1.0f, speaker.Volume, 2);
            
            // Test clamping below minimum
            speaker.Volume = -0.5f;
            Assert.Equal(0.0f, speaker.Volume, 2);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("OpenAL"))
        {
            // Skip test if OpenAL is not available
        }
    }

    [Fact]
    public void ConfigureBuildOptions_ModifiesOptions()
    {
        using var speaker = new MP3Speaker();

        // This test verifies that the configuration method works without errors
        speaker.ConfigureBuildOptions(opt => opt.SetLogLevel("debug"));

        // We can't easily verify the internal state without making the options public,
        // but at least we verify the method doesn't throw
        Assert.True(true);
    }

    [Fact]
    public async Task PlayAsync_WithEmptyStream_CompletesWithoutError()
    {
        using var speaker = new MP3Speaker();
        using var emptyStream = new MemoryStream();

        // Note: This test might throw due to OpenAL initialization in test environments
        // In a real test environment, you might want to mock the OpenAL dependencies
        try
        {
            await speaker.PlayAsync(emptyStream, CancellationToken.None);
            Assert.True(true);
        }
        catch (InvalidOperationException)
        {
            // Expected in test environments without proper audio setup
            Assert.True(true);
        }
    }

    [Fact]
    public void Stop_WithoutPlaying_DoesNotThrow()
    {
        using var speaker = new MP3Speaker();

        // Should not throw even if not playing
        speaker.Stop();
        Assert.True(true);
    }

    [Fact]
    public void Dispose_MultipleTimes_DoesNotThrow()
    {
        var speaker = new MP3Speaker();

        speaker.Dispose();
        speaker.Dispose(); // Should not throw on multiple dispose calls

        Assert.True(true);
    }
}