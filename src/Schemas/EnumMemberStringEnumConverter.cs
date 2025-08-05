using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aivis.Schemas;
/// <summary>
/// EnumMember属性をサポートするJsonStringEnumConverter
/// System.Text.JsonのデフォルトのJsonStringEnumConverterはEnumMember属性をサポートしていないため、
/// カスタム実装を提供します。
/// </summary>
public class EnumMemberStringEnumConverter : JsonConverterFactory
{
    // パフォーマンス向上のため、リフレクション結果をキャッシュ
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _enumValueCache 
        = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();
    
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, string>> _enumNameCache 
        = new ConcurrentDictionary<Type, ConcurrentDictionary<object, string>>();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (JsonConverter)Activator.CreateInstance(
            typeof(EnumMemberStringEnumConverterInner<>).MakeGenericType(typeToConvert))!;
    }

    private class EnumMemberStringEnumConverterInner<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected string value for enum {typeToConvert.Name}, but got {reader.TokenType}");
            }

            string? value = reader.GetString();
            if (value == null)
            {
                throw new JsonException($"Enum value cannot be null for type {typeToConvert.Name}");
            }

            // キャッシュから値を取得
            var valueCache = _enumValueCache.GetOrAdd(typeToConvert, BuildValueCache);
            
            if (valueCache.TryGetValue(value, out var enumValue))
            {
                return (T)enumValue;
            }

            throw new JsonException($"Unable to convert \"{value}\" to enum {typeToConvert.Name}. " +
                $"Valid values are: {string.Join(", ", valueCache.Keys)}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var nameCache = _enumNameCache.GetOrAdd(typeof(T), BuildNameCache);
            
            if (nameCache.TryGetValue(value, out var name))
            {
                writer.WriteStringValue(name);
            }
            else
            {
                // フォールバック: enum名をそのまま使用
                writer.WriteStringValue(value.ToString());
            }
        }

        private static ConcurrentDictionary<string, object> BuildValueCache(Type enumType)
        {
            var cache = new ConcurrentDictionary<string, object>();

            foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field.IsLiteral && field.FieldType == enumType)
                {
                    var enumValue = field.GetValue(null)!;
                    
                    // EnumMember属性の値を優先
                    var enumMemberAttr = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttr?.Value != null)
                    {
                        cache.TryAdd(enumMemberAttr.Value, enumValue);
                    }
                    
                    // フォールバック: フィールド名も登録（大文字小文字を区別しない）
                    cache.TryAdd(field.Name, enumValue);
                    
                    // enum.ToString()の結果も登録
                    cache.TryAdd(enumValue.ToString()!, enumValue);
                }
            }

            return cache;
        }

        private static ConcurrentDictionary<object, string> BuildNameCache(Type enumType)
        {
            var cache = new ConcurrentDictionary<object, string>();

            foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (field.IsLiteral && field.FieldType == enumType)
                {
                    var enumValue = field.GetValue(null)!;
                    
                    // EnumMember属性の値を優先
                    var enumMemberAttr = field.GetCustomAttribute<EnumMemberAttribute>();
                    if (enumMemberAttr?.Value != null)
                    {
                        cache.TryAdd(enumValue, enumMemberAttr.Value);
                    }
                    else
                    {
                        // フォールバック: フィールド名を使用
                        cache.TryAdd(enumValue, field.Name);
                    }
                }
            }

            return cache;
        }
    }
}

/// <summary>
/// 特定のenum型専用のコンバーター（パフォーマンスが重要な場合に使用）
/// </summary>
/// <typeparam name="T">enum型</typeparam>
public class EnumMemberStringEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private static readonly Lazy<Dictionary<string, T>> _valueMap = new Lazy<Dictionary<string, T>>(BuildValueMap);
    private static readonly Lazy<Dictionary<T, string>> _nameMap = new Lazy<Dictionary<T, string>>(BuildNameMap);

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Expected string value for enum {typeToConvert.Name}, but got {reader.TokenType}");
        }

        string? value = reader.GetString();
        if (value == null)
        {
            throw new JsonException($"Enum value cannot be null for type {typeToConvert.Name}");
        }

        if (_valueMap.Value.TryGetValue(value, out var enumValue))
        {
            return enumValue;
        }

        throw new JsonException($"Unable to convert \"{value}\" to enum {typeToConvert.Name}. " +
            $"Valid values are: {string.Join(", ", _valueMap.Value.Keys)}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (_nameMap.Value.TryGetValue(value, out var name))
        {
            writer.WriteStringValue(name);
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    private static Dictionary<string, T> BuildValueMap()
    {
        var map = new Dictionary<string, T>(StringComparer.Ordinal);
        var enumType = typeof(T);

        foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (field.IsLiteral && field.FieldType == enumType)
            {
                var enumValue = (T)field.GetValue(null)!;
                
                // EnumMember属性の値を優先
                var enumMemberAttr = field.GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttr?.Value != null)
                {
                    map.TryAdd(enumMemberAttr.Value, enumValue);
                }
                
                // フォールバック値も登録
                map.TryAdd(field.Name, enumValue);
                map.TryAdd(enumValue.ToString(), enumValue);
            }
        }

        return map;
    }

    private static Dictionary<T, string> BuildNameMap()
    {
        var map = new Dictionary<T, string>();
        var enumType = typeof(T);

        foreach (var field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (field.IsLiteral && field.FieldType == enumType)
            {
                var enumValue = (T)field.GetValue(null)!;
                
                // EnumMember属性の値を優先
                var enumMemberAttr = field.GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttr?.Value != null)
                {
                    map.TryAdd(enumValue, enumMemberAttr.Value);
                }
                else
                {
                    map.TryAdd(enumValue, field.Name);
                }
            }
        }

        return map;
    }
}