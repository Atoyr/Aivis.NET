using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Aivis.Schemas;

namespace Aivis;

public class AivisClient
{
    private AivisClientOptions _options;
    private readonly HttpClient _httpClient;

    public AivisClient(AivisClientOptions options)
    {
        _options = options.Clone();
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_options.ApiKey}");
    }

    public async Task<byte[]> TTS(string modelUuid, string text, string format = "mp3")
    {
        TTSRequest requestBody = new(modelUuid, text){ OutputFormat = format};
        var jsonContent = JsonSerializer.Serialize(requestBody);

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var apiUrl = GetApiUrl("/v1/tts/synthesize");

        var response = await _httpClient.PostAsync(apiUrl, content);

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
}