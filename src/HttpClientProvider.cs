namespace Aivis;

/// <summary>
/// HttpClientProvider クラスは、HttpClient の単一インスタンスを提供します。
/// </summary>
public static class HttpClientProvider
{
    /// <summary>
    /// HttpClient の共有インスタンス。
    /// </summary>
    public static readonly HttpClient Instance = new HttpClient();
}