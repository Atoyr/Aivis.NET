namespace Aivis;

/// <summary>
/// IHttpClientProvider インターフェイスは、HttpClient の共有インスタンスを提供します。
/// </summary>
public interface IHttpClientProvider
{
    /// <summary>
    /// HttpClient の共有インスタンス。
    /// </summary>
    HttpClient Instance { get; }
}
