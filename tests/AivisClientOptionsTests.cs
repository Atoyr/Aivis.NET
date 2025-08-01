namespace Aivis.Tests;

public class AivisClientOptionsTests
{
    [Fact]
    public void Constructor_ValidApiKey_SetsProperties()
    {
        var apiKey = "test-api-key";
        var options = new AivisClientOptions(apiKey);

        // InternalsVisibleToによりApiKeyにアクセス可能
        Assert.Equal(apiKey, options.ApiKey);
        Assert.Equal("https://api.aivis-project.com", options.BaseUrl);
    }

    [Fact]
    public void Constructor_CustomBaseUrl_SetsCustomUrl()
    {
        var apiKey = "test-api-key";
        var customUrl = "https://custom-api.example.com";
        var options = new AivisClientOptions(apiKey)
        {
            BaseUrl = customUrl
        };

        Assert.Equal(apiKey, options.ApiKey);
        Assert.Equal(customUrl, options.BaseUrl);
    }

    [Fact]
    public void Constructor_CustomHttpClientProvider_SetsCustomHttpClientPrvider()
    {
        var apiKey = "test-api-key";
        var customUrl = "https://custom-api.example.com";
        var httpClient = new HttpClient();
        var httpClientProvider = new MockHttpClientProvider(httpClient);
        var options = new AivisClientOptions(apiKey)
        {
            BaseUrl = customUrl, 
            HttpClientProvider = httpClientProvider
        };

        Assert.Equal(apiKey, options.ApiKey);
        Assert.Equal(customUrl, options.BaseUrl);
        Assert.Equal(httpClientProvider, options.HttpClientProvider);
    }

    [Fact]
    public void Clone_CreatesExactCopy()
    {
        var originalApiKey = "original-api-key";
        var customUrl = "https://custom-api.example.com";
        var httpClient = new HttpClient();
        var httpClientProvider = new MockHttpClientProvider(httpClient);
        var original = new AivisClientOptions(originalApiKey)
        {
            BaseUrl = customUrl, 
            HttpClientProvider = httpClientProvider
        };

        var cloned = original.Clone();

        Assert.Equal(original.ApiKey, cloned.ApiKey);
        Assert.Equal(original.BaseUrl, cloned.BaseUrl);
        Assert.Equal(original.HttpClientProvider, cloned.HttpClientProvider);
        Assert.NotSame(original, cloned);
    }

    [Fact]
    public void Clone_IndependentCopies()
    {
        var original = new AivisClientOptions("test-key");
        var cloned = original.Clone();

        // Modify the cloned instance
        var modifiedClone = new AivisClientOptions("test-key")
        {
            BaseUrl = "https://modified-url.com", 
        };

        // Original should remain unchanged
        Assert.Equal("https://api.aivis-project.com", original.BaseUrl);
        Assert.Equal("https://modified-url.com", modifiedClone.BaseUrl);
    }
}