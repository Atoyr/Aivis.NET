namespace Aivis;

/// <summary>
/// AivisClientOptions クラスは、Aivis API クライアントの設定を管理します。
/// </summary>
public class AivisClientOptions
{
    /// <summary>
    /// API キー。
    /// </summary>
    internal string ApiKey { init; get; }

    /// <summary>
    /// API のベース URL。
    /// </summary>
    public string BaseUrl { init; get; } = "https://api.aivis-project.com";

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
        return new AivisClientOptions(ApiKey)
        {
            BaseUrl = BaseUrl
        };
    }
}