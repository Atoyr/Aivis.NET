using System.Reflection;
using System.Web;

namespace Aivis;

public static class HttpHelper
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
        {
            return string.Empty;
        }

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
        return string.Empty;
    }


    /// <summary>
    /// エンドポイントとクエリオブジェクトからURLを構築します
    /// </summary>
    /// <param name="endpoint">APIエンドポイント</param>
    /// <param name="query">クエリパラメータとして使用するオブジェクト</param>
    /// <returns>構築されたURL</returns>
    public static string BuildUrl(string endpoint, object? query)
    {
        var baseUrl = endpoint.TrimEnd('/');

        if (query == null)
        {
            return baseUrl;
        }

        var queryParams = BuildQueryParameters(query);
        if (queryParams.Count == 0)
        {
            return baseUrl;
        }

        var queryString = string.Join("&", queryParams.Select(kvp =>
            $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}"));

        return $"{baseUrl}?{queryString}";
    }

    /// <summary>
    /// オブジェクトからクエリパラメータのディクショナリを構築します
    /// </summary>
    /// <param name="query">クエリオブジェクト</param>
    /// <returns>クエリパラメータのディクショナリ</returns>
    private static Dictionary<string, string> BuildQueryParameters(object query)
    {
        var parameters = new Dictionary<string, string>();

        if (query == null)
        {
            return parameters;
        }

        var properties = query.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(query);

            // nullの場合はスキップ
            if (value == null)
            {
                continue;
            }

            var propertyName = GetPropertyName(property);

            // 配列の場合
            if (value is Array array)
            {
                AddArrayParameters(parameters, propertyName, array);
            }
            // その他の値型・文字列の場合
            else
            {
                var stringValue = value.ToString();
                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    parameters[propertyName] = stringValue;
                }
            }
        }

        return parameters;
    }

    /// <summary>
    /// プロパティ名を取得します（camelCaseに変換）
    /// </summary>
    /// <param name="property">プロパティ情報</param>
    /// <returns>プロパティ名</returns>
    private static string GetPropertyName(PropertyInfo property)
    {
        // プロパティ名をスネークケースに変換
        var name = property.Name;
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }
        return ToSnakeCase(name);
    }

    /// <summary>
    /// PascalCaseやcamelCaseの文字列をスネークケースに変換します
    /// </summary>
    /// <param name="input">変換元の文字列</param>
    /// <returns>スネークケース文字列</returns>
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                if (i > 0) sb.Append('_');
                sb.Append(char.ToLowerInvariant(input[i]));
            }
            else
            {
                sb.Append(input[i]);
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// 配列パラメータを追加します
    /// </summary>
    /// <param name="parameters">パラメータディクショナリ</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="array">配列値</param>
    private static void AddArrayParameters(Dictionary<string, string> parameters, string propertyName, Array array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var item = array.GetValue(i);
            if (item != null)
            {
                var stringValue = item.ToString();
                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    // 配列の場合は複数のパラメータとして追加
                    // 例: tags[0]=value1&tags[1]=value2 または tags=value1&tags=value2
                    // ここでは後者の形式を採用（一般的なWebAPIの形式）
                    parameters[$"{propertyName}[{i}]"] = stringValue;
                }
            }
        }
    }
}