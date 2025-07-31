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

    /// <summary>
    /// Content-Dispositionヘッダーからファイル名を抽出します。
    /// </summary>
    /// <param name="contentDisposition">Content-Dispositionヘッダーの値</param>
    /// <returns>ファイル名</returns>
    public static string ExtractFileName(string contentDisposition)
    {
        if (string.IsNullOrEmpty(contentDisposition))
            return string.Empty;
            
        // 様々なパターンに対応
        // 例: "inline; filename="file.mp3""
        // 例: "attachment; filename=file.mp3"
        // 例: "inline; filename*=UTF-8''file.mp3"
        
        var parts = contentDisposition.Split(';');
        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            
            // 通常のfilename=パターン
            if (trimmed.StartsWith("filename=", StringComparison.OrdinalIgnoreCase))
            {
                var fileName = trimmed.Substring("filename=".Length).Trim();
                return fileName.Trim('"').Trim('\'');
            }
            
            // RFC 5987準拠のfilename*=パターン（エンコードされたファイル名）
            if (trimmed.StartsWith("filename*=", StringComparison.OrdinalIgnoreCase))
            {
                var encodedFileName = trimmed.Substring("filename*=".Length).Trim();
                // UTF-8''filename.mp3 のような形式から filename.mp3 を抽出
                var lastQuoteIndex = encodedFileName.LastIndexOf('\'');
                if (lastQuoteIndex >= 0 && lastQuoteIndex < encodedFileName.Length - 1)
                {
                    return encodedFileName.Substring(lastQuoteIndex + 1);
                }
                return encodedFileName.Trim('"').Trim('\'');
            }
        }
        return null;
    }
}