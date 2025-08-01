using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;

namespace Aivis.Tests;

public class AivisTTSClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AivisClientOptions _options;

    public AivisTTSClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        HttpClientProvider.SetFactory(() => _httpClient);
        _options = new AivisClientOptions("test-api-key");
    }

    [Fact]
    public void Constructor_ValidOptions_CreatesInstance()
    {
        var client = new AivisTTSClient(_options);
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AivisTTSClient(null!));
    }

    [Fact]
    public void Constructor_EmptyApiKey_ThrowsArgumentException()
    {
        var options = new AivisClientOptions("");
        Assert.Throws<ArgumentException>(() => new AivisTTSClient(options));
    }

    [Fact]
    public void Constructor_NullApiKey_ThrowsArgumentException()
    {
        var options = new AivisClientOptions(null!);
        Assert.Throws<ArgumentException>(() => new AivisTTSClient(options));
    }

    [Fact]
    public async Task SynthesizeAsync_ValidRequest_ReturnsAudioData()
    {
        var expectedAudioData = Encoding.UTF8.GetBytes("fake audio data");
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(expectedAudioData)
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        var result = await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト");

        Assert.Equal(expectedAudioData, result);
        VerifyHttpRequest("/v1/tts/synthesize");
    }

    [Fact]
    public async Task SynthesizeAsync_WithFormat_UsesSpecifiedFormat()
    {
        var expectedAudioData = Encoding.UTF8.GetBytes("fake audio data");
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(expectedAudioData)
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト", "wav");

        VerifyHttpRequest("/v1/tts/synthesize", requestBody =>
        {
            var request = JsonSerializer.Deserialize<JsonElement>(requestBody);
            return request.GetProperty("output_format").GetString() == "wav";
        });
    }

    [Fact]
    public async Task SynthesizeWithContentsAsync_ValidRequest_ReturnsTTSContents()
    {
        var expectedAudioData = Encoding.UTF8.GetBytes("fake audio data");
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(expectedAudioData)
        };
        
        responseMessage.Content.Headers.Add("Content-Disposition", "attachment; filename=\"test.mp3\"");
        responseMessage.Headers.Add("X-Aivis-Billing-Mode", "PayAsYouGo");
        responseMessage.Headers.Add("X-Aivis-Character-Count", "10");
        responseMessage.Headers.Add("X-Aivis-Credits-Remaining", "100");
        responseMessage.Headers.Add("X-Aivis-Credits-Used", "5");
        responseMessage.Headers.Add("X-Aivis-Rate-Limit-Remaining", "50");

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        var result = await client.SynthesizeWithContentsAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト");

        Assert.Equal(expectedAudioData, result.Audio);
        Assert.Equal("test.mp3", result.ContentDisposition);
        Assert.Equal("PayAsYouGo", result.BillingMode);
        Assert.Equal(10u, result.CharacterCount);
        Assert.Equal(100u, result.CreditsRemaining);
        Assert.Equal(5u, result.CreditsUsed);
        Assert.Equal(50u, result.RateLimitRemaining);
        Assert.True(result.IsPayAsYouGo);
        Assert.False(result.IsSubscription);
    }

    [Theory]
    [InlineData(HttpStatusCode.Unauthorized, typeof(UnauthorizedAccessException))]
    [InlineData(HttpStatusCode.PaymentRequired, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.NotFound, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.UnprocessableContent, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.TooManyRequests, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.InternalServerError, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.BadGateway, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.ServiceUnavailable, typeof(NotSupportedException))]
    [InlineData(HttpStatusCode.GatewayTimeout, typeof(NotSupportedException))]
    public async Task SynthesizeAsync_ErrorStatusCode_ThrowsExpectedException(
        HttpStatusCode statusCode, 
        Type expectedExceptionType)
    {
        var responseMessage = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent("Error message")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        
        var exception = await Assert.ThrowsAsync(expectedExceptionType, async () =>
            await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト"));
        
        Assert.Contains(statusCode.ToString(), exception.Message);
    }

    [Fact]
    public async Task SynthesizeAsync_UnknownErrorStatusCode_ThrowsException()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.Conflict)
        {
            Content = new StringContent("Unknown error")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        
        var exception = await Assert.ThrowsAsync<Exception>(async () =>
            await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト"));
        
        Assert.Contains("音声合成に失敗しました", exception.Message);
        Assert.Contains("Conflict", exception.Message);
    }

    [Fact]
    public async Task SynthesizeAsync_SendsCorrectRequest()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes("audio"))
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト", "wav");

        VerifyHttpRequest("/v1/tts/synthesize", requestBody =>
        {
            var request = JsonSerializer.Deserialize<JsonElement>(requestBody);
            return request.GetProperty("model_uuid").GetString() == "550e8400-e29b-41d4-a716-446655440000" &&
                   request.GetProperty("text").GetString() == "テストテキスト" &&
                   request.GetProperty("output_format").GetString() == "wav";
        });
    }

    [Fact]
    public async Task SynthesizeAsync_CustomBaseUrl_UsesCorrectUrl()
    {
        var customOptions = new AivisClientOptions("test-api-key")
        {
            BaseUrl = "https://custom-api.example.com"
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes("audio"))
        };

        SetupHttpResponse(responseMessage, "https://custom-api.example.com/v1/tts/synthesize");

        var client = new AivisTTSClient(customOptions);
        await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト");

        VerifyHttpRequest("/v1/tts/synthesize", expectedBaseUrl: "https://custom-api.example.com");
    }

    [Fact]
    public async Task SynthesizeAsync_IncludesAuthorizationHeader()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(Encoding.UTF8.GetBytes("audio"))
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisTTSClient(_options);
        await client.SynthesizeAsync("550e8400-e29b-41d4-a716-446655440000", "テストテキスト");

        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => 
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "test-api-key"),
                ItExpr.IsAny<CancellationToken>());
    }

    private void SetupHttpResponse(HttpResponseMessage responseMessage, string? expectedUrl = null)
    {
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                expectedUrl != null 
                    ? ItExpr.Is<HttpRequestMessage>(req => req.RequestUri!.ToString() == expectedUrl)
                    : ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);
    }

    private void VerifyHttpRequest(string expectedPath, Func<string, bool>? requestBodyValidator = null, string? expectedBaseUrl = null)
    {
        var baseUrl = expectedBaseUrl ?? "https://api.aivis-project.com";
        var expectedUrl = baseUrl + expectedPath;

        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString() == expectedUrl &&
                    req.Content!.Headers.ContentType!.MediaType == "application/json" &&
                    (requestBodyValidator == null || ValidateRequestBody(req, requestBodyValidator))),
                ItExpr.IsAny<CancellationToken>());
    }

    private static bool ValidateRequestBody(HttpRequestMessage request, Func<string, bool> validator)
    {
        var content = request.Content!.ReadAsStringAsync().Result;
        return validator(content);
    }
}