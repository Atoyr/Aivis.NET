namespace Aivis;

/// <summary>
/// HttpClientProvider クラスは、HttpClient の単一インスタンスを提供します。
/// </summary>
public class HttpClientProvider : IHttpClientProvider
{
    private HttpClient _instance = new HttpClient();

    /// <summary>
    /// HttpClient の共有インスタンス。
    /// </summary>
    public HttpClient Instance => _instance;
}