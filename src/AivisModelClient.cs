using System.Net;
using System.Net.Http.Json;

using Aivis.Schemas;

namespace Aivis;

/// <summary>
/// Aivisが提供するAIVMモデルを操作するためのクライアントです。
/// </summary>
public class AivisModelClient
{
    private readonly AivisClientOptions _options;

    private const string ModelEndpoint = "/v1/aivm-models";

    private string SearchEndpoint() => GetApiUrl($"{ModelEndpoint}/search");

    private string DetailsEndpoint(string modelId) => GetApiUrl($"{ModelEndpoint}/{modelId}");

    private string DownloadEndpoint(string modelId) => GetApiUrl($"{ModelEndpoint}/{modelId}/download");

    /// <summary>
    /// AivisModelClientのコンストラクタ。
    /// </summary>
    /// <param name="options">AivisClientOptionsオブジェクト。APIキーとHTTPクライアントプロバイダを含む。</param>
    public AivisModelClient(AivisClientOptions options)
    {
        _options = options.Clone();
    }

    /// <summary>
    /// Aivmモデルを検索します。
    /// </summary>
    /// <param name="options">検索オプションを含むSearchVoiceModelsOptionsオブジェクト。</param>
    /// <returns>AivmModelsResponseオブジェクト。検索結果を含む。</returns>
    public async Task<AivmModelsResponse> SearchModels(SearchVoiceModelsOptions options)
    {
        var response = await _options.HttpClientProvider.Instance.GetAsync(SearchEndpoint(), options);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<AivmModelsResponse>() ?? new AivmModelsResponse(0, Enumerable.Empty<AivmModelResponse>());
        }

        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            var error = await response.Content.ReadFromJsonAsync<HttpValidationError>();
            throw new HttpRequestException($"Validation error: {string.Join(", ", error?.Detail?.Select(e => e.Msg) ?? Enumerable.Empty<string>())}");
        }

        throw new HttpRequestException($"Failed to search models: {response.ReasonPhrase}");
    }

    /// <summary>
    /// Aivmモデルの詳細情報を取得します。
    /// </summary>
    /// <param name="modelId">モデルID</param>
    /// <returns>AivmModelResponseオブジェクト。</returns>
    public async Task<AivmModelResponse> GetModelDetail(string modelId)
    {
        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));
        }

        var response = await _options.HttpClientProvider.Instance.GetAsync(DetailsEndpoint(modelId));

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return await response.Content.ReadFromJsonAsync<AivmModelResponse>() ?? throw new InvalidOperationException("Received empty model details.");
        }

        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            var error = await response.Content.ReadFromJsonAsync<HttpValidationError>();
            throw new HttpRequestException($"Validation error: {string.Join(", ", error?.Detail?.Select(e => e.Msg) ?? Enumerable.Empty<string>())}");
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException($"Model with ID '{modelId}' not found.");
        }

        throw new HttpRequestException($"Failed to get model details: {response.ReasonPhrase}");
    }

    /// <summary>
    /// ModelのダウンロードURLを取得します。
    /// </summary>
    public async Task<string> GetDownloadUrl(string modelId, ModelType modelType = ModelType.AIVM)
    {
        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));
        }

        var response = await _options.HttpClientProvider.Instance.GetAsync(DownloadEndpoint(modelId), new { ModelType = modelType, Redirect = false });
        if (response.StatusCode == HttpStatusCode.Created)
        {
            // LocationヘッダからURLを取得
            if (response.Headers.Location is not null)
            {
                return response.Headers.Location.ToString();
            }
            throw new HttpRequestException("Locationヘッダが存在しません。");
        }

        if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            var error = await response.Content.ReadFromJsonAsync<HttpValidationError>();
            throw new HttpRequestException($"Validation error: {string.Join(", ", error?.Detail?.Select(e => e.Msg) ?? Enumerable.Empty<string>())}");
        }

        // その他の処理（詳細取得等）
        throw new HttpRequestException($"Failed to get model download url: {response.StatusCode} : {response.ReasonPhrase}");
    }

    private string GetApiUrl(string path) => _options.BaseUrl.TrimEnd('/') + path;
}