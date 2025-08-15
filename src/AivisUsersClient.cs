using System.Net;
using System.Net.Http.Json;

using Aivis.Schemas;

namespace Aivis;

public class AivisUsersClient
{
    private readonly AivisClientOptions _options;

    private const string ModelEndpoint = "/v1/users";

    private string MeEndpoint() => GetApiUrl($"{ModelEndpoint}/me");

    private string UserInfoEndpoint(string handleName) => GetApiUrl($"{ModelEndpoint}/{handleName}");

    public AivisUsersClient(AivisClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options.Clone();
    }

    public async Task<UserResponseForMe> GetMe()
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new ArgumentException("APIキーが設定されていません。AivisClientOptionsのApiKeyプロパティを設定してください。", nameof(_options));
        }

        var response = await _options.HttpClientProvider.Instance.GetWithAuthAsync(_options.ApiKey, MeEndpoint());

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<UserResponseForMe>() ?? throw new InvalidOperationException("Received empty user details.");
        }

        // OKレスポンス以外
        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedAccessException("APIキーが無効です。AivisClientOptionsのApiKeyプロパティを確認してください。");
            default:
                var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new HttpRequestException($"Failed to get me: {response.StatusCode}: {errorResponse?.Detail ?? response.ReasonPhrase}");
        }
    }

    public async Task<UserResponseWithAivmModels> GetUserInfo(string handleName)
    {
        var response = await _options.HttpClientProvider.Instance.GetAsync(UserInfoEndpoint(handleName));

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<UserResponseWithAivmModels>() ?? throw new InvalidOperationException("Received empty user details.");
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedAccessException("APIキーが無効です。AivisClientOptionsのApiKeyプロパティを確認してください。");
            case HttpStatusCode.NotFound:
                var notFoundError = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new NotSupportedException($"{response.StatusCode} - {notFoundError?.Detail ?? "ユーザーが見つかりませんでした。"}");
            case HttpStatusCode.UnprocessableContent:
                var validationError = await response.Content.ReadFromJsonAsync<HttpValidationError>();
                throw new HttpRequestException($"Validation error: {string.Join(", ", validationError?.Detail?.Select(e => e.Msg) ?? Enumerable.Empty<string>())}");
            default:
                var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                throw new HttpRequestException($"Failed to get user info: {response.StatusCode}: {errorResponse?.Detail ?? response.ReasonPhrase}");
        }
    }

    private string GetApiUrl(string path) => _options.BaseUrl.TrimEnd('/') + path;
}