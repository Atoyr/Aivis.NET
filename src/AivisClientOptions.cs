namespace Aivis;

/// <summary>
/// AivisClientOptions クラスは、Aivis API クライアントの設定を管理します。
/// </summary>
public class AivisClientOptions
{
    /// <summary>
    /// API キー。
    /// </summary>
    internal string? ApiKey { init; get; }

    /// <summary>
    /// API のベース URL。
    /// </summary>
    public string BaseUrl { init; get; } = "https://api.aivis-project.com";

    /// <summary>
    /// HTTP クライアントプロバイダー。
    /// </summary>
    public IHttpClientProvider HttpClientProvider { init; get; } = new HttpClientProvider();

    /// <summary>
    /// 新しい AivisClientOptions クラスのインスタンスを初期化します。
    /// </summary>
    public AivisClientOptions()
    {
        ApiKey = null;
    }
    /// <summary>
    /// 新しい AivisClientOptions クラスのインスタンスを初期化します。
    /// </summary>
    /// <param name="apiKey">API キー。</param>
    public AivisClientOptions(string apiKey)
    {
        ApiKey = apiKey;
    }

    /// <summary>
    /// AivisClientOptions のクローンを作成します。
    /// </summary>
    /// <returns>クローンされた AivisClientOptions。</returns>
    public AivisClientOptions Clone()
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            return new AivisClientOptions()
            {
                BaseUrl = BaseUrl,
                HttpClientProvider = HttpClientProvider
            };
        }
        else
        {
            return new AivisClientOptions(ApiKey)
            {
                BaseUrl = BaseUrl,
                HttpClientProvider = HttpClientProvider
            };
        }
    }
}