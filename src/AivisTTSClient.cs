using System.Text;
using System.Text.Json;

using Aivis.Schemas;

namespace Aivis;

public class AivisTTSClient : ITalkToSpeech
{
    private AivisClientOptions _options;

    private const string SynthetizeEndpoint = "/v1/tts/synthesize";

    public AivisTTSClient(AivisClientOptions options)
    {
        if (string.IsNullOrEmpty(options?.ApiKey))
        {
            throw new ArgumentException("APIキーが設定されていません。AivisClientOptionsのApiKeyプロパティを設定してください。", nameof(options));
        }
        _options = options.Clone();
    }

    /// <inheritdoc />
    public async Task<byte[]> Synthesize(string modelUuid, string text, string format = "mp3")
    {
        TTSRequest requestBody = new(modelUuid, text){ OutputFormat = format};
        var jsonContent = JsonSerializer.Serialize(requestBody);

        var response = await PostWithAuthAsync(SynthetizeEndpoint, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            // ストリーミングレスポンスの処理
            return await response.Content.ReadAsByteArrayAsync();
        }
        else
        {
            throw new Exception($"音声合成に失敗しました: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
        }
    }

    private string GetApiUrl(string path) => _options.BaseUrl.TrimEnd('/') + path;

    private async Task<HttpResponseMessage> PostWithAuthAsync(string endpoint, string json)
    {
        HttpRequestMessage request = new(HttpMethod.Post, GetApiUrl(endpoint))
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        request.Headers.Add("Authorization", $"Bearer {_options.ApiKey}");
        return await HttpClientProvider.Instance.SendAsync(request);
    }
}