using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

using Aivis.Schemas;

namespace Aivis;

public class AivisModelClient 
{
    private readonly AivisClientOptions _options;

    private const string ModelEndpoint = "/v1/aivm-models";

    private string SearchEndpoint() => GetApiUrl($"{ModelEndpoint}/search");

    private string DetailsEndpoint(string modelId) => GetApiUrl($"{ModelEndpoint}/{modelId}");

    public AivisModelClient(AivisClientOptions options)
    {
        _options = options.Clone();
    }

    // FIXME: 
    public async Task<AivmModelsResponse> SearchModels(SearchVoiceModelsOptions options)
    {
        var response = await GetAsync(SearchEndpoint(), options);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new HttpRequestException($"Failed to search models: {response.ReasonPhrase}");
        }

        return await response.Content.ReadFromJsonAsync<AivmModelsResponse>() ?? new AivmModelsResponse(0, Enumerable.Empty<AivmModelResponse>());
    }

    private string GetApiUrl(string path) => _options.BaseUrl.TrimEnd('/') + path;


    /// <summary>
    /// GETリクエストを送信し、クエリオブジェクトをクエリパラメータに変換します
    /// </summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="query">クエリパラメータとして使用するオブジェクト</param>
    /// <returns>HTTPレスポンス</returns>
    private async Task<HttpResponseMessage> GetAsync(string endpoint, object? query = null)
    {
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));
        }

        var url = HttpHelper.BuildUrl(endpoint, query);
        Console.WriteLine(url);
        return await _options.HttpClientProvider.Instance.GetAsync(url);
    }

}