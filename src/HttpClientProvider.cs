namespace Aivis;

/// <summary>
/// HttpClientProvider クラスは、HttpClient の単一インスタンスを提供します。
/// </summary>
public class HttpClientProvider : IHttpClientProvider, IDisposable
{
    private readonly HttpClient _instance = new HttpClient();

    /// <summary>
    /// HttpClient の共有インスタンス。
    /// </summary>
    public HttpClient Instance => _instance;

    public HttpClientProvider() { }

    public void Dispose()
    {
        _instance?.Dispose();
    }
}