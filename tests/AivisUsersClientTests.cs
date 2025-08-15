using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Aivis.Schemas;

using Moq;
using Moq.Protected;

namespace Aivis.Tests;

public class AivisUsersClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AivisClientOptions _options;
    private readonly IHttpClientProvider _httpClientProvider;

    public AivisUsersClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _httpClientProvider = new MockHttpClientProvider(_httpClient);
        _options = new AivisClientOptions("test-api-key")
        {
            HttpClientProvider = _httpClientProvider,
        };
    }

    [Fact]
    public void Constructor_ValidOptions_CreatesInstance()
    {
        var client = new AivisUsersClient(_options);
        Assert.NotNull(client);
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new AivisUsersClient(null!));
    }

    [Fact]
    public async Task GetMe_ValidApiKey_ReturnsUserResponseForMe()
    {
        var expectedResponse = new UserResponseForMe(
            Handle: "testuser",
            Name: "Test User",
            Description: "Test Description",
            IconUrl: "https://example.com/icon.png",
            BirthYear: 1990,
            Gender: Gender.Other,
            Email: "test@example.com",
            AccountType: AccountType.User,
            AccountStatus: AccountStatus.Active,
            SocialLinks: new[]
            {
                new SocialLink(SocialLinkType.Twitter, "https://twitter.com/testuser")
            },
            CreditBalance: 1000,
            IsLowBalanceNotificationEnabled: true,
            LowBalanceThreshold: 100,
            LikedAivmModels: Array.Empty<AivmModelsResponse>(),
            AivmModels: Array.Empty<AivmModelsResponse>(),
            CreatedAt: DateTime.Parse("2024-01-01T00:00:00Z"),
            UpdatedAt: DateTime.Parse("2024-01-02T00:00:00Z")
        );

        var responseJson = JsonSerializer.Serialize(expectedResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);
        var result = await client.GetMe();

        Assert.Equal(expectedResponse.Handle, result.Handle);
        Assert.Equal(expectedResponse.Name, result.Name);
        Assert.Equal(expectedResponse.Email, result.Email);
        Assert.Equal(expectedResponse.CreditBalance, result.CreditBalance);
        VerifyHttpGetWithAuthRequest("https://api.aivis-project.com/v1/users/me");
    }

    [Fact]
    public async Task GetMe_EmptyApiKey_ThrowsArgumentException()
    {
        var optionsWithEmptyKey = new AivisClientOptions("")
        {
            HttpClientProvider = _httpClientProvider
        };

        var client = new AivisUsersClient(optionsWithEmptyKey);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.GetMe());

        Assert.Contains("APIキーが設定されていません", exception.Message);
    }

    [Fact]
    public async Task GetMe_NullApiKey_ThrowsArgumentException()
    {
        var optionsWithNullKey = new AivisClientOptions(null!)
        {
            HttpClientProvider = _httpClientProvider
        };

        var client = new AivisUsersClient(optionsWithNullKey);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.GetMe());

        Assert.Contains("APIキーが設定されていません", exception.Message);
    }

    [Fact]
    public async Task GetMe_WhitespaceApiKey_ThrowsArgumentException()
    {
        var optionsWithWhitespaceKey = new AivisClientOptions("   ")
        {
            HttpClientProvider = _httpClientProvider
        };

        var client = new AivisUsersClient(optionsWithWhitespaceKey);

        var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () => await client.GetMe());

        Assert.Contains("APIキーが設定されていません", exception.Message);
    }

    [Fact]
    public async Task GetMe_EmptyResponse_ThrowsInvalidOperationException()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await client.GetMe());

        Assert.Contains("Received empty user details", exception.Message);
    }

    [Fact]
    public async Task GetMe_UnauthorizedResponse_ThrowsUnauthorizedAccessException()
    {
        var errorResponse = new ErrorResponse("Unauthorized access");
        var responseJson = JsonSerializer.Serialize(errorResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            async () => await client.GetMe());

        Assert.Contains("APIキーが無効です", exception.Message);
    }

    [Fact]
    public async Task GetMe_OtherErrorResponse_ThrowsHttpRequestException()
    {
        var errorResponse = new ErrorResponse("Internal server error");
        var responseJson = JsonSerializer.Serialize(errorResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            async () => await client.GetMe());

        Assert.Contains("Failed to get me", exception.Message);
        Assert.Contains("InternalServerError", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_ValidHandleName_ReturnsUserResponseWithAivmModels()
    {
        var expectedResponse = new UserResponseWithAivmModels(
            Handle: "targetuser",
            Name: "Target User",
            Description: "Target Description",
            IconUrl: "https://example.com/target.png",
            AccountType: AccountType.User,
            AccountStatus: AccountStatus.Active,
            SocialLinks: new[]
            {
                new SocialLink(SocialLinkType.YouTube, "https://youtube.com/targetuser")
            },
            AivmModels: Array.Empty<AivmModelsResponse>()
        );

        var responseJson = JsonSerializer.Serialize(expectedResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);
        var result = await client.GetUserInfo("targetuser");

        Assert.Equal(expectedResponse.Handle, result.Handle);
        Assert.Equal(expectedResponse.Name, result.Name);
        Assert.Equal(expectedResponse.Description, result.Description);
        VerifyHttpGetRequest("https://api.aivis-project.com/v1/users/targetuser");
    }

    [Fact]
    public async Task GetUserInfo_EmptyResponse_ThrowsInvalidOperationException()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await client.GetUserInfo("testuser"));

        Assert.Contains("Received empty user details", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_UnauthorizedResponse_ThrowsUnauthorizedAccessException()
    {
        var errorResponse = new ErrorResponse("Unauthorized access");
        var responseJson = JsonSerializer.Serialize(errorResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
            async () => await client.GetUserInfo("testuser"));

        Assert.Contains("APIキーが無効です", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_NotFoundResponse_ThrowsNotSupportedException()
    {
        var errorResponse = new ErrorResponse("User not found");
        var responseJson = JsonSerializer.Serialize(errorResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await client.GetUserInfo("nonexistentuser"));

        Assert.Contains("NotFound", exception.Message);
        Assert.Contains("User not found", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_NotFoundResponseNoDetail_ThrowsNotSupportedException()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<NotSupportedException>(
            async () => await client.GetUserInfo("nonexistentuser"));

        Assert.Contains("NotFound", exception.Message);
        Assert.Contains("ユーザーが見つかりませんでした", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_ValidationErrorResponse_ThrowsHttpRequestException()
    {
        var validationError = new HttpValidationError(new[]
        {
            new HttpValidationErrorDetail(new object[] { "handleName" }, "Invalid handle name format", "string"),
            new HttpValidationErrorDetail(new object[] { "handleName" }, "Handle name too long", "length")
        });

        var responseJson = JsonSerializer.Serialize(validationError);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.UnprocessableContent)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            async () => await client.GetUserInfo("invalid-handle"));

        Assert.Contains("Validation error", exception.Message);
        Assert.Contains("Invalid handle name format", exception.Message);
        Assert.Contains("Handle name too long", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_OtherErrorResponse_ThrowsHttpRequestException()
    {
        var errorResponse = new ErrorResponse("Internal server error");
        var responseJson = JsonSerializer.Serialize(errorResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        var exception = await Assert.ThrowsAsync<HttpRequestException>(
            async () => await client.GetUserInfo("testuser"));

        Assert.Contains("Failed to get user info", exception.Message);
        Assert.Contains("InternalServerError", exception.Message);
    }

    [Fact]
    public async Task GetUserInfo_CustomBaseUrl_UsesCorrectUrl()
    {
        var customOptions = new AivisClientOptions("test-api-key")
        {
            BaseUrl = "https://custom-api.example.com",
            HttpClientProvider = _httpClientProvider
        };

        var expectedResponse = new UserResponseWithAivmModels(
            Handle: "testuser",
            Name: "Test User",
            Description: "Test Description",
            IconUrl: "https://example.com/icon.png",
            AccountType: AccountType.User,
            AccountStatus: AccountStatus.Active,
            SocialLinks: Array.Empty<SocialLink>(),
            AivmModels: Array.Empty<AivmModelsResponse>()
        );

        var responseJson = JsonSerializer.Serialize(expectedResponse);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage, "https://custom-api.example.com/v1/users/testuser");

        var client = new AivisUsersClient(customOptions);
        await client.GetUserInfo("testuser");

        VerifyHttpGetRequest("https://custom-api.example.com/v1/users/testuser");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public async Task GetUserInfo_InvalidHandleName_CallsApiWithInvalidName(string handleName)
    {
        var validationError = new HttpValidationError(new[]
        {
            new HttpValidationErrorDetail(new object[] { "handleName" }, "Invalid handle name", "string")
        });
        var responseJson = JsonSerializer.Serialize(validationError);
        var responseMessage = new HttpResponseMessage(HttpStatusCode.UnprocessableContent)
        {
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };

        SetupHttpResponse(responseMessage);

        var client = new AivisUsersClient(_options);

        await Assert.ThrowsAsync<HttpRequestException>(
            async () => await client.GetUserInfo(handleName));
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

    private void VerifyHttpGetRequest(string expectedUrl)
    {
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString() == expectedUrl),
                ItExpr.IsAny<CancellationToken>());
    }

    private void VerifyHttpGetWithAuthRequest(string expectedUrl)
    {
        _mockHttpMessageHandler.Protected()
            .Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri!.ToString() == expectedUrl &&
                    req.Headers.Authorization != null &&
                    req.Headers.Authorization.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "test-api-key"),
                ItExpr.IsAny<CancellationToken>());
    }
}