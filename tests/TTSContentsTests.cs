namespace Aivis.Tests;

public class TTSContentsTests
{
    [Fact]
    public void TTSContents_PayAsYouGo_IsPayAsYouGoReturnsTrue()
    {
        using var audioStream = new MemoryStream(new byte[] { 1, 2, 3 });
        var contents = new TTSContents(
            audioStream,
            "test.mp3",
            "PayAsYouGo",
            100
        );

        Assert.True(contents.IsPayAsYouGo);
        Assert.False(contents.IsSubscription);
    }

    [Fact]
    public void TTSContents_Subscription_IsSubscriptionReturnsTrue()
    {
        using var audioStream = new MemoryStream(new byte[] { 1, 2, 3 });
        var contents = new TTSContents(
            audioStream,
            "test.mp3",
            "Subscription",
            100
        );

        Assert.False(contents.IsPayAsYouGo);
        Assert.True(contents.IsSubscription);
    }

    [Fact]
    public void TTSContents_OtherBillingMode_BothReturnFalse()
    {
        using var audioStream = new MemoryStream(new byte[] { 1, 2, 3 });
        var contents = new TTSContents(
            audioStream,
            "test.mp3",
            "Unknown",
            100
        );

        Assert.False(contents.IsPayAsYouGo);
        Assert.False(contents.IsSubscription);
    }

    [Fact]
    public void TTSContents_DefaultValues_AreSetCorrectly()
    {
        using var audioStream = new MemoryStream(new byte[] { 1, 2, 3 });
        var contents = new TTSContents(
            audioStream,
            "test.mp3",
            "PayAsYouGo",
            100
        );

        Assert.Equal(0u, contents.CreditsRemaining);
        Assert.Equal(0u, contents.CreditsUsed);
        Assert.Equal(0u, contents.RateLimitRemaining);
    }

    [Fact]
    public void TTSContents_AllParameters_AreSetCorrectly()
    {
        using var audioStream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5 });
        var contents = new TTSContents(
            audioStream,
            "test.wav",
            "Subscription",
            250,
            500,
            10,
            100
        );

        Assert.Equal(audioStream, contents.AudioStream);
        Assert.Equal("test.wav", contents.ContentDisposition);
        Assert.Equal("Subscription", contents.BillingMode);
        Assert.Equal(250u, contents.CharacterCount);
        Assert.Equal(500u, contents.CreditsRemaining);
        Assert.Equal(10u, contents.CreditsUsed);
        Assert.Equal(100u, contents.RateLimitRemaining);
    }
}