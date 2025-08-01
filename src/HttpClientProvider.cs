namespace Aivis;

/// <summary>
/// HttpClientProvider クラスは、HttpClient の単一インスタンスを提供します。
/// </summary>
public static class HttpClientProvider
{
    private static HttpClient _instance = new HttpClient();

    /// <summary>
    /// HttpClient の共有インスタンス。
    /// </summary>
    public static HttpClient Instance => _instance;

    /// <summary>
    /// テスト用にHttpClientファクトリを設定します。
    /// </summary>
    /// <param name="factory">HttpClientを作成するファクトリ関数</param>
    internal static void SetFactory(Func<HttpClient> factory)
    {
        _instance = factory();
    }

    /// <summary>
    /// HttpClientインスタンスをリセットして既定の状態に戻します。
    /// </summary>
    internal static void Reset()
    {
        _instance = new HttpClient();
    }
}