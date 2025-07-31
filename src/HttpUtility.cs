namespace Aivis;

public static class HttpUtility
{
    /// <summary>
    /// ヘッダーから指定されたキーの値を取得します。
    /// </summary>
    /// <param name="response">HTTPレスポンス</param>
    /// <param name="key">取得するヘッダーのキー</param>
    /// <returns>ヘッダーの値。存在しない場合は空文字列。</returns>
    public static string GetHeaderValue(HttpResponseMessage response, string key)
    {
        return response.Headers.TryGetValues(key, out var values) ? values.FirstOrDefault() ?? string.Empty : string.Empty;
    }

    /// <summary>
    /// ヘッダーから指定されたキーの値を取得します。
    /// </summary>
    /// <param name="response">HTTPレスポンス</param>
    /// <param name="key">取得するヘッダーのキー</param>
    /// <returns>int型のヘッダーの値。存在しない場合は0。</returns>
    public static int GetHeaderValueAsInt(HttpResponseMessage response, string key)
    {
        var value = GetHeaderValue(response, key);
        return int.TryParse(value, out var result) ? result : 0;
    }

    /// <summary>
    /// ヘッダーから指定されたキーの値を取得します。
    /// </summary>
    /// <param name="response">HTTPレスポンス</param>
    /// <param name="key">取得するヘッダーのキー</param>
    /// <returns>uint型のヘッダーの値。存在しない場合は0。</returns>
    public static uint GetHeaderValueAsUInt(HttpResponseMessage response, string key)
    {
        var value = GetHeaderValue(response, key);
        return uint.TryParse(value, out var result) ? result : 0;
    }
}