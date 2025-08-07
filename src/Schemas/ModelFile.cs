using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// モデルファイル
/// </summary>
public record ModelFile(

    /// <summary>
    /// AIVMモデルUUID
    /// </summary>
    [property: JsonPropertyName("aivm_model_uuid")]
    Guid AivmModelUuid,

    /// <summary>
    /// マニフェスト バージョン
    /// </summary>
    [property: JsonPropertyName("manifest_version")]
    string ManifestVersion,

    /// <summary>
    /// 名前
    /// </summary>
    [property: JsonPropertyName("name")]
    string Name,

    /// <summary>
    /// 説明
    /// </summary>
    [property: JsonPropertyName("description")]
    string Description,

    /// <summary>
    /// クリエイター
    /// </summary>
    [property: JsonPropertyName("creators")]
    string[] Creators,

    /// <summary>
    /// ライセンス種別
    /// </summary>
    [property: JsonPropertyName("license_type")]
    LicenseType LicenseType,

    /// <summary>
    /// ライセンス本文
    /// </summary>
    [property: JsonPropertyName("license_text")]
    string? LicenseText,

    /// <summary>
    /// モデル区分
    /// </summary>
    [property: JsonPropertyName("model_type")]
    ModelType ModelType,

    /// <summary>
    /// モデル アーキテクチャ
    /// </summary>
    [property: JsonPropertyName("model_architecture")]
    ModelArchitecture ModelArchitecture,

    /// <summary>
    /// モデル フォーマット
    /// </summary>
    [property: JsonPropertyName("model_format")]
    ModelFormat ModelFormat,

    /// <summary>
    /// 学習世代
    /// </summary>
    [property: JsonPropertyName("training_epoches")]
    int? TrainingEpochs,

    /// <summary>
    /// 学習ステップ数
    /// </summary>
    [property: JsonPropertyName("training_steps")]
    int? TrainingSteps,

    /// <summary>
    /// バージョン
    /// </summary>
    [property: JsonPropertyName("version")]
    string Version,

    /// <summary>
    /// ファイルサイズ
    /// </summary>
    [property: JsonPropertyName("file_size")]
    int FileSize,

    /// <summary>
    /// チェックサム
    /// </summary>
    [property: JsonPropertyName("checksum")]
    string Checksum,

    /// <summary>
    /// ダウンロード数
    /// </summary>
    [property: JsonPropertyName("download_count")]
    int DownloadCount,

    /// <summary>
    /// 作成日
    /// </summary>
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,

    /// <summary>
    /// 更新日
    /// </summary>
    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);