namespace Aivis;

/// <summary>
/// Talk to Speech (TTS) APIのオプションを定義するクラスです。
/// </summary>
public class TalkToSpeechOptions
{
    /// <summary>
    /// 音声合成モデルの「話者 UUID」を指定します。ここで指定された話者を音声合成に利用します。
    /// 話者 UUID は、AivmManifest.speakers[].uuid の値を指定します。
    ///
    /// 単一話者モデルでは指定の必要はありません。 当該モデルに存在しない話者の UUID を指定すると 422 エラーが発生します。
    /// 複数話者モデルでこの値が省略された場合は、当該モデルのデフォルト話者を音声合成に利用します。
    /// </summary>
    public Guid? SpeakerUuid { get; set; }

    private int styleId = 0;
    /// <summary>
    /// 音声合成モデルの話者のスタイル ID を 0 ~ 31 の範囲で指定します。通常、ノーマルスタイルの ID は 0 です。
    /// スタイル ID には、AivmManifest.speakers[].styles[].local_id の値を指定します。 style_name とは併用できません。
    /// 
    /// 当該モデル → 話者 → スタイルに存在しないスタイル ID を指定すると 422 エラーが発生します。
    /// 未指定時は、当該話者のノーマルスタイルを音声合成に利用します。
    /// </summary>
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

    /// <summary>
    /// 指定されたユーザー辞書 UUID に対応するユーザー辞書を、音声合成時に適用します。
    /// 
    /// ユーザー辞書を利用するには、事前にユーザー辞書 API を通してユーザー辞書を作成しておく必要があります。
    /// 未指定時は、デフォルト辞書のみを適用した状態で音声合成を行います。
    /// </summary>
    public Guid? UserDictionaryUuid { get; set; }

    /// <summary>
    /// text に記述された SSML タグのサブセットの解釈を有効にするかを指定します。デフォルトで有効です。
    /// 現在、SSML のサブセット (&lt;break time/strength=&quot;...&quot;&gt;, &lt;prosody rate/pitch/volume=&quot;...&quot;&gt;, &lt;sub alias=&quot;...&quot;&gt;, &lt;p&gt;, &lt;s&gt;) にのみ対応しています。
    /// 
    /// true の場合、text の内容を SSML (XML) として解釈します。タグとして解釈されうる制御文字はエスケープが必要です。
    /// false の場合、text の内容はすべてプレーンテキストとして扱われ、SSML タグは解釈しません。
    /// </summary>
    public bool UseSsml { get; set; } = true;


    /// <summary>
    /// 音声データの出力形式を指定します。用途に応じて最適な形式を選択してください。
    /// wav, flac, mp3, aac, opus
    /// </summary>
    public MediaType OutputFormat { get; set; } = MediaType.MP3;

    /// <summary>
    /// TalkToSpeechOptions のデフォルトコンストラクタです。
    /// </summary>
    public TalkToSpeechOptions()
    {
        // デフォルトコンストラクタ
    }

    /// <summary>
    /// スピーカーのUUIDを設定します。
    /// 単一話者の場合設定は不要です。
    /// </summary>
    /// <param name="speakerUuid">スピーカーのUUID</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetSpeakerUuid(string? speakerUuid)
    {
        if (speakerUuid is null)
        {
            return SetSpeakerUuid((Guid?)null);
        }

        if (Guid.TryParse(speakerUuid, out var uuid))
        {
            return SetSpeakerUuid(uuid);
        }
        else
        {
            throw new ArgumentException("Invalid UUID format", nameof(speakerUuid));
        }
    }

    /// <summary>
    /// スピーカーのUUIDを設定します。
    /// 単一話者の場合設定は不要です。
    /// </summary>
    /// <param name="speakerUuid">スピーカーのUUID</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetSpeakerUuid(Guid? speakerUuid)
    {
        SpeakerUuid = speakerUuid;
        return this;
    }

    /// <summary>
    /// 話者スタイルを設定します。
    /// </summary>
    /// <param name="styleId">話者スタイル</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetStyle(int styleId)
    {
        StyleId = styleId;
        return this;
    }

    /// <summary>
    /// ユーザー辞書UUIDを設定します。
    /// </summary>
    /// <param name="userDictionaryUuid">ユーザー辞書のUUID</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetUserDictionary(string? userDictionaryUuid)
    {
        if (userDictionaryUuid is null)
        {
            return SetUserDictionary((Guid?)null);
        }

        if (Guid.TryParse(userDictionaryUuid, out var uuid))
        {
            return SetUserDictionary(uuid);
        }
        else
        {
            throw new ArgumentException("Invalid UUID format", nameof(userDictionaryUuid));
        }
    }

    /// <summary>
    /// ユーザー辞書UUIDを設定します。
    /// </summary>
    /// <param name="userDictionaryUuid">ユーザー辞書のUUID</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetUserDictionary(Guid? userDictionaryUuid)
    {
        UserDictionaryUuid = userDictionaryUuid;
        return this;
    }

    /// <summary>
    /// Ssmlの使用を設定します。
    /// </summary>
    /// <param name="useSsml">Ssmlを使用する場合はtrue</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetUseSsml(bool useSsml = true)
    {
        UseSsml = useSsml;
        return this;
    }

    /// <summary>
    /// 出力するファイルフォーマットを設定します。
    /// </summary>
    /// <param name="format">出力フォーマット</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetOutputFormat(MediaType format)
    {
        OutputFormat = format;
        return this;
    }
}