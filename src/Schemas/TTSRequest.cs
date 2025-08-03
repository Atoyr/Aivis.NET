using System.Text.Json.Serialization;

namespace Aivis.Schemas;

public class TTSRequest
{
    private Guid modelUuid;
    /// <summary>
    /// 音声合成モデルの「モデル UUID」を指定します。ここで指定された音声合成モデルを音声合成に利用します。
    /// モデル UUID は、AivmManifest.uuid の値を指定します。
    /// </summary>
    [JsonPropertyName("model_uuid")]
    public string ModelUuid
    {
        get => modelUuid.ToString("D");
        set
        {
            if (Guid.TryParse(value, out var uuid))
            {
                modelUuid = uuid;
            }
            else
            {
                throw new ArgumentException("Invalid UUID format", nameof(value));
            }
        }
    }

    private Guid? speakerUuid = null;
    /// <summary>
    /// 音声合成モデルの「話者 UUID」を指定します。ここで指定された話者を音声合成に利用します。
    /// 話者 UUID は、AivmManifest.speakers[].uuid の値を指定します。
    ///
    /// 単一話者モデルでは指定の必要はありません。 当該モデルに存在しない話者の UUID を指定すると 422 エラーが発生します。
    /// 複数話者モデルでこの値が省略された場合は、当該モデルのデフォルト話者を音声合成に利用します。
    /// </summary>
    [JsonPropertyName("speaker_uuid")]
    public string? SpeakerUuid
    {
        get => speakerUuid?.ToString("D");
        set
        {
            if (value is null)
            {
                speakerUuid = null;
                return;
            }

            if (Guid.TryParse(value, out var uuid))
            {
                speakerUuid = uuid;
            }
            else
            {
                throw new ArgumentException("Invalid UUID format", nameof(value));
            }
        }
    }

    private int styleId = 0;
    /// <summary>
    /// 音声合成モデルの話者のスタイル ID を 0 ~ 31 の範囲で指定します。通常、ノーマルスタイルの ID は 0 です。
    /// スタイル ID には、AivmManifest.speakers[].styles[].local_id の値を指定します。 style_name とは併用できません。
    /// 
    /// 当該モデル → 話者 → スタイルに存在しないスタイル ID を指定すると 422 エラーが発生します。
    /// 未指定時は、当該話者のノーマルスタイルを音声合成に利用します。
    /// </summary>
    [JsonPropertyName("style_id")]
    public int StyleId
    {
        get => styleId;
        set
        {
            if (0 <= value && value <= 31)
            {
                styleId = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "StyleId must be between 0 and 31.");
            }
        }
    }

    // FIXME: StyleIdと併用できない
    // private string? styleName = null;
    // /// <summary>
    // /// 音声合成モデルの話者のスタイル名を指定します。通常、ノーマルスタイルの名前は「ノーマル」です。
    // /// スタイル名には、AivmManifest.speakers[].styles[].name の値を指定します。 style_id とは併用できません。
    // /// 
    // /// 当該モデル → 話者 → スタイルに存在しないスタイル名を指定すると 422 エラーが発生します。
    // /// 未指定時は、当該話者のデフォルトスタイルを音声合成に利用します。
    // /// </summary>
    // [JsonPropertyName("style_name")]
    // public string? StyleName 
    // {
    //     get => styleName ?? "ノーマル";
    //     set => styleName = value;
    // }


    private Guid? userDictionaryUuid = null;
    /// <summary>
    /// 指定されたユーザー辞書 UUID に対応するユーザー辞書を、音声合成時に適用します。
    /// 
    /// ユーザー辞書を利用するには、事前にユーザー辞書 API を通してユーザー辞書を作成しておく必要があります。
    /// 未指定時は、デフォルト辞書のみを適用した状態で音声合成を行います。
    /// </summary>
    [JsonPropertyName("user_dictionary_uuid")]
    public string? UserDictionaryUuid
    {
        get => userDictionaryUuid?.ToString("D");
        set
        {
            if (value is null)
            {
                userDictionaryUuid = null;
                return;
            }

            if (Guid.TryParse(value, out var uuid))
            {
                userDictionaryUuid = uuid;
            }
            else
            {
                throw new ArgumentException("Invalid UUID format", nameof(value));
            }
        }
    }

    /// <summary>
    /// 
    /// 読み上げるテキストを指定します。最大文字数は 3000 文字です。
    /// 
    /// 改行ごとに別々に音声合成を行い、それらを結合した音声データを出力します。通常、1〜3文程度で区切ったほうが自然な音声になりやすい傾向にあります。
    /// なお、意味的につながりのある文章や自然な感情表現を重視する場合は、段落や文脈の区切りまでを一行にまとめることで、より滑らかで自然な読み上げができます。
    /// 一行が 400 文字を超える場合、400 文字に近い文末記号（「。」「．」「！」「？」「(全角スペース)」）の前後で分割した上で音声合成を行います。
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// text に記述された SSML タグのサブセットの解釈を有効にするかを指定します。デフォルトで有効です。
    /// 現在、SSML のサブセット (<break time/strength="...">, <prosody rate/pitch/volume="...">, <sub alias="...">, <p>, <s>) にのみ対応しています。
    /// 
    /// true の場合、text の内容を SSML (XML) として解釈します。タグとして解釈されうる制御文字はエスケープが必要です。
    /// false の場合、text の内容はすべてプレーンテキストとして扱われ、SSML タグは解釈しません。
    /// </summary>
    [JsonPropertyName("use_ssml")]
    public bool UseSsml { get; set; } = true;


    /// <summary>
    /// 音声データの出力形式を指定します。用途に応じて最適な形式を選択してください。
    /// wav, flac, mp3, aac, opus
    /// </summary>
    [JsonPropertyName("output_format")]
    public string OutputFormat { get; set; } = "mp3";

    public TTSRequest(string modelUuid, string text)
    {
        ModelUuid = modelUuid;
        Text = text;
    }
}