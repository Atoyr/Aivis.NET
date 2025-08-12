using System.Net;
using System.Text;
using System.Text.Json;

using Aivis.Schemas;

namespace Aivis;

public class AivisTTSClient : ITalkToSpeech
{
    private readonly AivisClientOptions _options;

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
    public async Task<byte[]> SynthesizeAsync(string modelUuid, string text, string format = "mp3")
    {
        var response = await PostSynthesizeAsync(modelUuid, text, format);
        return await response.Content.ReadAsByteArrayAsync();
    }

    /// <inheritdoc />
    public async Task<Stream> SynthesizeStreamAsync(string modelUuid, string text, string format = "mp3")
    {
        var response = await PostSynthesizeAsync(modelUuid, text, format);
        return await response.Content.ReadAsStreamAsync();
    }

    /// <inheritdoc />
    public async Task<TTSContents> SynthesizeWithContentsAsync(string modelUuid, string text, string format = "mp3")
    {
        var response = await PostSynthesizeAsync(modelUuid, text, format);
        var audioStream = response.Content.ReadAsStreamAsync();

        // Content-Dispositionヘッダーの取得
        string contentDisposition = response.Content.Headers.ContentDisposition?.ToString() ?? string.Empty;
        string fileName = HttpHelper.ExtractFileName(contentDisposition);

        // カスタムヘッダーの取得
        string billingMode = HttpHelper.GetHeaderValue(response, "X-Aivis-Billing-Mode");
        uint characterCount = HttpHelper.GetHeaderValueAsUInt(response, "X-Aivis-Character-Count");
        uint creditsRemaining = HttpHelper.GetHeaderValueAsUInt(response, "X-Aivis-Credits-Remaining");
        uint creditsUsed = HttpHelper.GetHeaderValueAsUInt(response, "X-Aivis-Credits-Used");
        uint rateLimitRemaining = HttpHelper.GetHeaderValueAsUInt(response, "X-Aivis-Rate-Limit-Remaining");

        return new TTSContents(
                await audioStream,
                fileName,
                billingMode,
                characterCount,
                creditsRemaining,
                creditsUsed,
                rateLimitRemaining
                );
    }

    private async Task<HttpResponseMessage> PostSynthesizeAsync(string modelUuid, string text, string format = "mp3")
    {
        TTSRequest requestBody = new(modelUuid, text) { OutputFormat = format };
        var jsonContent = JsonSerializer.Serialize(requestBody);

        var response = await PostWithAuthAsync(SynthetizeEndpoint, jsonContent);
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedAccessException($"{response.StatusCode} - APIキーが設定されていないか無効です。");
            case HttpStatusCode.PaymentRequired:
                throw new NotSupportedException($"{response.StatusCode} - クレジット残高が不足しています。");
            case HttpStatusCode.NotFound:
                throw new NotSupportedException($"{response.StatusCode} - 指定されたモデルが見つかりません。モデルUUIDを確認してください。");
            case HttpStatusCode.UnprocessableContent:
                throw new NotSupportedException($"{response.StatusCode} - リクエストパラメータの形式が正しくありません。");
            case HttpStatusCode.TooManyRequests:
                throw new NotSupportedException($"{response.StatusCode} - 音声合成APIのレート制限に到達しました。");
            case HttpStatusCode.InternalServerError:
                throw new NotSupportedException($"{response.StatusCode} - Citorasサーバへの接続中に不明なエラーが発生しました。");
            case HttpStatusCode.BadGateway:
                throw new NotSupportedException($"{response.StatusCode} - Citorasサーバへの接続に失敗しました。");
            case HttpStatusCode.ServiceUnavailable:
                throw new NotSupportedException($"{response.StatusCode} - Citorasサーバで障害が発生しているか、音声合成に失敗しました。");
            case HttpStatusCode.GatewayTimeout:
                throw new NotSupportedException($"{response.StatusCode} - Citorasサーバへの接続がタイムアウトしました。");
            default:
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
        return await _options.HttpClientProvider.Instance.SendAsync(request);
    }
}