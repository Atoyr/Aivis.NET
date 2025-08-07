using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// AIVMモデルのレスポンスモデルのリスト
/// </summary>
public record AivmModelsResponse
(
    /// <summary>
    /// AIVMモデルの総数
    /// </summary>
    [property: JsonPropertyName("total")]
    int Total,

    /// <summary>
    /// AIVMモデルの配列
    /// 必ずしも総数と数は一致しない
    /// </summary>
    [property: JsonPropertyName("aivm_models")]
    IEnumerable<AivmModelResponse> AivmModels
);