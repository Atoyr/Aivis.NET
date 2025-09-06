using System.Text.Json.Serialization;

namespace Aivis.Schemas;

/// <summary>
/// TTSRequest クラスは、音声合成リクエストのパラメータを定義します。
/// </summary>
public class TTSRequest
{
    private Guid _modelUuid;
    /// <summary>
    /// 音声合成モデルの「モデル UUID」を指定します。ここで指定された音声合成モデルを音声合成に利用します。
    /// モデル UUID は、AivmManifest.uuid の値を指定します。
    /// </summary>
    [JsonPropertyName("model_uuid")]
    public string ModelUuid
    {
        get => _modelUuid.ToString("D");
        set
        {
            if (Guid.TryParse(value, out var uuid))
            {
                _modelUuid = uuid;
            }
            else
            {
                throw new ArgumentException("Invalid UUID format", nameof(value));
            }
        }
    }

    private Guid? _speakerUuid = null;
    /// <summary>
    /// 音声合成モデルの「話者 UUID」を指定します。ここで指定された話者を音声合成に利用します。
    /// 話者 UUID は、AivmManifest.speakers[].uuid の値を指定します。
    ///
    /// 単一話者モデルでは指定の必要はありません。 当該モデルに存在しない話者の UUID を指定すると 422 エラーが発生します。
    /// 複数話者モデルでこの値が省略された場合は、当該モデルのデフォルト話者を音声合成に利用します。
    /// </summary>
    [JsonPropertyName("speaker_uuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SpeakerUuid
    {
        get => _speakerUuid?.ToString("D");
        set
        {
            if (value is null)
            {
                _speakerUuid = null;
                return;
            }

            if (Guid.TryParse(value, out var uuid))
            {
                _speakerUuid = uuid;
            }
            else
            {
                throw new ArgumentException("Invalid UUID format", nameof(value));
            }
        }
    }

    /// <summary>
    /// スタイルIDのデフォルト値: 0
    /// </summary>
    public const int DEFAULT_STYLE_ID = 0;
    private int _styleId = DEFAULT_STYLE_ID;
    /// <summary>
    /// 音声合成モデルの話者のスタイル ID を 0 ~ 31 の範囲で指定します。通常、ノーマルスタイルの ID は 0 です。
    /// スタイル ID には、AivmManifest.speakers[].styles[].local_id の値を指定します。 style_name とは併用できません。
    /// 
    /// 当該モデル → 話者 → スタイルに存在しないスタイル ID を指定すると 422 エラーが発生します。
    /// 未指定時は、当該話者のノーマルスタイルを音声合成に利用します。
    /// </summary>
    [JsonIgnore]
    public int StyleId
    {
        get => _styleId;
        set
        {
            if (0 <= value && value <= 31)
            {
                _styleId = (int)value;
                StyleName = DEFAULT_STYLE_NAME;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "StyleId must be between 0 and 31.");
            }
        }
    }

    [JsonPropertyName("style_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private int? _styleIdRaw
    {
        get => StyleId != DEFAULT_STYLE_ID ? StyleId : null;
        set => StyleId = value ?? DEFAULT_STYLE_ID;
    }

    /// <summary>
    /// スタイル名のデフォルト値: ノーマル
    /// </summary>
    public const string DEFAULT_STYLE_NAME = "ノーマル";
    private string _styleName = DEFAULT_STYLE_NAME;
    /// <summary>
    /// 音声合成モデルの話者のスタイル名を指定します。通常、ノーマルスタイルの名前は「ノーマル」です。
    /// スタイル名には、AivmManifest.speakers[].styles[].name の値を指定します。 style_id とは併用できません。
    /// 
    /// 当該モデル → 話者 → スタイルに存在しないスタイル名を指定すると 422 エラーが発生します。
    /// 未指定時は、当該話者のデフォルトスタイルを音声合成に利用します。
    /// </summary>
    [JsonIgnore]
    public string StyleName
    {
        get
        {
            return _styleName;
        }
        set
        {
            // デフォルト値以外はStyleIdをデフォルト値に変更する
            if (value != DEFAULT_STYLE_NAME)
            {
                StyleId = DEFAULT_STYLE_ID;
                _styleName = value;
                return;
            }

            _styleName = DEFAULT_STYLE_NAME;
        }
    }

    [JsonPropertyName("style_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private string? _styleNameRaw
    {
        get => StyleName != DEFAULT_STYLE_NAME ? StyleName : null;
        set => StyleName = value ?? DEFAULT_STYLE_NAME;
    }

    private Guid? _userDictionaryUuid = null;
    /// <summary>
    /// 指定されたユーザー辞書 UUID に対応するユーザー辞書を、音声合成時に適用します。
    /// 
    /// ユーザー辞書を利用するには、事前にユーザー辞書 API を通してユーザー辞書を作成しておく必要があります。
    /// 未指定時は、デフォルト辞書のみを適用した状態で音声合成を行います。
    /// </summary>
    [JsonPropertyName("user_dictionary_uuid")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserDictionaryUuid
    {
        get
        {
            if (_userDictionaryUuid is null || _userDictionaryUuid == Guid.Empty)
            {
                return null;
            }
            return _userDictionaryUuid?.ToString("D");
        }
        set
        {
            if (value is null)
            {
                _userDictionaryUuid = null;
                return;
            }

            if (Guid.TryParse(value, out var uuid))
            {
                _userDictionaryUuid = uuid;
            }
            else
            {
                throw new ArgumentException("Invalid UUID format", nameof(value));
            }
        }
    }

    /// <summary>
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
    /// 現在、SSML のサブセット (&lt;break time/strength=&quot;...&quot;&gt;, &lt;prosody rate/pitch/volume=&quot;...&quot;&gt;, &lt;sub alias=&quot;...&quot;&gt;, &lt;p&gt;, &lt;s&gt;) にのみ対応しています。
    /// 
    /// true の場合、text の内容を SSML (XML) として解釈します。タグとして解釈されうる制御文字はエスケープが必要です。
    /// false の場合、text の内容はすべてプレーンテキストとして扱われ、SSML タグは解釈しません。
    /// </summary>
    [JsonPropertyName("use_ssml")]
    public bool UseSsml { get; set; } = true;

    /// <summary>
    /// 言語設定のデフォルト値: ja
    /// </summary>
    public const string DEFAULT_LANGUAGE = "ja";
    private string _language = DEFAULT_LANGUAGE;
    /// <summary>
    /// テキスト読み上げ時の言語を BCP 47 言語タグで指定します。2025/08時点では日本語のみ対応しています。
    /// 未指定時は、デフォルトの日本語で音声合成を行います。
    /// </summary>
    [JsonIgnore]
    public string Language
    {
        get => _language;
        set => _language = value;
    }

    [JsonPropertyName("launguage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private string? _languageRaw
    {
        get => Language != DEFAULT_LANGUAGE ? Language : null;
        set => Language = value ?? DEFAULT_LANGUAGE;
    }


    /// <summary>
    /// 話す早さのデフォルト値: 1.0
    /// </summary>
    public const double DEFAULT_SPEAKING_RATE = 1.0;
    private double _speakingRate = DEFAULT_SPEAKING_RATE;
    /// <summary>
    /// 話す速さを 0.5 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 2.0 で 2 倍速、0.5 で 0.5 倍速になります。
    /// </summary>
    [JsonIgnore]
    public double SpeakingRate
    {
        get => _speakingRate;
        set
        {
            if (value < 0.5 || value > 2.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "speakingRate must be between 0.5 and 2.0.");
            }
            _speakingRate = value;
        }
    }

    [JsonPropertyName("speaking_rate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _speakingRateRaw
    {
        get => SpeakingRate != DEFAULT_SPEAKING_RATE ? SpeakingRate : null;
        set => SpeakingRate = value ?? DEFAULT_SPEAKING_RATE;
    }

    /// <summary>
    /// 感情表現の強弱のデフォルト値: 1.0
    /// </summary>
    public const double DEFAULT_EMOTION_INTENSITY = 1.0;
    private double emotionIntensity = DEFAULT_EMOTION_INTENSITY;
    /// <summary>
    /// 選択した話者スタイルの感情表現の強弱を 0.0 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 数値が大きいほど、選択した話者スタイルに近い感情表現が込められた声になります。
    /// 例えば話者スタイルが「上機嫌」なら、数値が大きいほどより嬉しそうな明るい話し方になります。
    /// 
    /// 一方で、話者やスタイルによっては、数値を上げすぎると発声がおかしくなったり、棒読みで不自然な声になる場合もあります。
    /// 正しく発声できる上限値は話者やスタイルごとに異なります。必要に応じて最適な値を見つけて調整してください。
    /// 
    /// ⚠️ ノーマルスタイルを利用する場合、emotional_intensity (話者スタイルの感情表現の強さ) は指定しても効果がありません。
    /// ノーマルスタイルは全スタイルの平均的な特徴を持つため、感情表現の強さは自動で最適化されます。
    /// </summary>
    [JsonIgnore]
    public double EmotionIntensity
    {
        get => emotionIntensity;
        set
        {
            if (value < 0.0 || value > 2.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "emotionIntensity must be between 0.0 and 2.0.");
            }
            emotionIntensity = value;
        }
    }

    [JsonPropertyName("emotion_intensity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _emotionIntensityRaw
    {
        get => EmotionIntensity != DEFAULT_EMOTION_INTENSITY ? EmotionIntensity : null;
        set => EmotionIntensity = value ?? DEFAULT_EMOTION_INTENSITY;
    }

    /// <summary>
    /// 話す早さのデフォルト値: 1.0
    /// </summary>
    public const double DEFAULT_TEMPO_DYNAMICS = 1.0;
    private double tempoDynamics = DEFAULT_TEMPO_DYNAMICS;
    /// <summary>
    /// 話す速さの緩急の強弱を 0.0 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 数値が大きいほど、より早口で生っぽい抑揚がついた声になります。
    /// 声の表現を細かく変化させたい際に調整してみてください。
    /// </summary>
    [JsonIgnore]
    public double TempoDynamics
    {
        get => tempoDynamics;
        set
        {
            if (value < 0.0 || value > 2.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "TempoDynamics must be between 0.0 and 2.0.");
            }
            tempoDynamics = value;
        }
    }

    [JsonPropertyName("tempo_dynamics")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _tempoDynamicsRaw
    {
        get => TempoDynamics != DEFAULT_TEMPO_DYNAMICS ? TempoDynamics : null;
        set => TempoDynamics = value ?? DEFAULT_TEMPO_DYNAMICS;
    }

    /// <summary>
    /// 声のピッチのデフォルト値: 0.0
    /// </summary>
    public const double DEFAULT_PITCH = 0.0;
    private double pitch = DEFAULT_PITCH;
    /// <summary>
    /// 声のピッチの高さ（音高）を -1.0 ~ 1.0 の範囲で指定します。(デフォルト: 0.0)
    /// 数値が大きいほど、より高いピッチの声になります。
    /// 
    /// ⚠️ 仕様上、この値を 0.0 から変更すると、合成音声の品質が劣化する場合があります。また、生成速度が大幅に低下します。
    /// クリアで高品質な音声を生成したいケースではなるべく指定しないことをおすすめします。
    /// </summary>
    [JsonIgnore]
    public double Pitch
    {
        get => pitch;
        set
        {
            if (value < -1.0 || value > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "pitch must be between -1.0 and 1.0.");
            }
            pitch = value;
        }
    }

    [JsonPropertyName("pitch")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _pitchRaw
    {
        get => Pitch != DEFAULT_PITCH ? Pitch : null;
        set => Pitch = value ?? DEFAULT_PITCH;
    }

    /// <summary>
    /// 音量のデフォルト値: 1.0
    /// </summary>
    public const double DEFAULT_VOLUME = 1.0;
    private double _volume = DEFAULT_VOLUME;
    /// <summary>
    /// 全体の音量の大きさを 0.0 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 数値が大きいほど、より大きな声になります。
    /// </summary>
    [JsonIgnore]
    public double Volume
    {
        get => _volume;
        set
        {
            if (value < 0.0 || value > 2.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "volume must be between 0.0 and 2.0.");
            }
            _volume = value;
        }
    }

    [JsonPropertyName("volume")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _volumeRaw
    {
        get => Volume != DEFAULT_VOLUME ? Volume : null;
        set => Volume = value ?? DEFAULT_VOLUME;
    }

    /// <summary>
    /// 音声先頭の無音区間のサイズのデフォルト値: 0.1
    /// </summary>
    public const double DEFAULT_LEADING_SILENCE_SECONDS = 0.1;
    private double _leadingSilenceSeconds = DEFAULT_LEADING_SILENCE_SECONDS;
    /// <summary>
    /// 音声先頭の無音時間の長さを秒単位で指定します。(デフォルト: 0.1)
    /// 0.0 を指定すると、音声先頭の無音時間を完全に削除できます。
    /// ストリーミング再生時に 0.0 を指定すると、再生開始までの待機時間をさらに削減できます。
    /// </summary>
    [JsonIgnore]
    public double LeadingSilenceSeconds
    {
        get => _leadingSilenceSeconds;
        set
        {
            if (value < 0.0 || value > 60.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "leading_silence_seconds must be between 0.0 and 60.0.");
            }
            _leadingSilenceSeconds = value;
        }
    }

    [JsonPropertyName("leading_silence_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _leadingSilenceSecondsRaw
    {
        get => LeadingSilenceSeconds != DEFAULT_LEADING_SILENCE_SECONDS ? LeadingSilenceSeconds : null;
        set => LeadingSilenceSeconds = value ?? DEFAULT_LEADING_SILENCE_SECONDS;
    }

    /// <summary>
    /// 音声末尾の無音区間のサイズのデフォルト値: 0.1
    /// </summary>
    public const double DEFAULT_TRAILING_SILENCE_SECONDS = 0.1;
    private double _trailingSilenceSeconds = DEFAULT_TRAILING_SILENCE_SECONDS;
    /// <summary>
    /// 音声末尾の無音時間の長さを秒単位で指定します。(デフォルト: 0.1)
    /// 0.0 を指定すると、音声末尾の無音時間を完全に削除できます。
    /// </summary>
    [JsonIgnore]
    public double TrailingSilenceSeconds
    {
        get => _trailingSilenceSeconds;
        set
        {
            if (value < 0.0 || value > 60.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "trailing_silence_seconds must be between 0.0 and 60.0.");
            }
            _trailingSilenceSeconds = value;
        }
    }

    [JsonPropertyName("trailing_silence_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _trailingSilenceSecondsRaw
    {
        get => TrailingSilenceSeconds != DEFAULT_TRAILING_SILENCE_SECONDS ? TrailingSilenceSeconds : null;
        set => TrailingSilenceSeconds = value ?? DEFAULT_TRAILING_SILENCE_SECONDS;
    }

    /// <summary>
    /// テキストの改行ごとに挟む無音区間の長さのデフォルト値: 0.4
    /// </summary>
    public const double DEFAULT_LINE_BREAK_SILENCE_SECONDS = 0.4;
    private double _lineBreakSilenceSeconds = DEFAULT_LINE_BREAK_SILENCE_SECONDS;
    /// <summary>
    /// テキストの改行ごとに挟む無音区間の長さを秒単位で指定します。(デフォルト: 0.4)
    /// 0.0 を指定すると、改行間の無音時間を完全に削除できます。
    /// 
    /// ⚠️ SSML が有効かつ &lt;p&gt;, &lt;s&gt; が記述されている場合、line_break_silence_seconds の値は適用されません。
    /// 必要に応じて &lt;break time="..."&gt; タグで無音区間をさらに調整可能です。
    /// </summary>
    [JsonIgnore]
    public double LineBreakSilenceSeconds
    {
        get => _lineBreakSilenceSeconds;
        set
        {
            if (value < 0.0 || value > 60.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "line_break_silence_seconds must be between 0.0 and 60.0.");
            }
            _lineBreakSilenceSeconds = value;
        }
    }

    [JsonPropertyName("line_break_silence_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private double? _lineBreakSilenceSecondsRaw
    {
        get => LineBreakSilenceSeconds != DEFAULT_LINE_BREAK_SILENCE_SECONDS ? LineBreakSilenceSeconds : null;
        set => LineBreakSilenceSeconds = value ?? DEFAULT_LINE_BREAK_SILENCE_SECONDS;
    }

    /// <summary>
    /// 出力フォーマットのデフォルト値: mp3
    /// </summary>
    public const string DEFAULT_OUTPUT_FORMAT = "mp3";
    private string _outputFormat = DEFAULT_OUTPUT_FORMAT;
    /// <summary>
    /// 音声データの出力形式を指定します。用途に応じて最適な形式を選択してください。
    ///
    /// wav: PCM 無圧縮 (audio/wav)
    /// - 音声合成エンジンの出力を無劣化・最高音質で出力します。
    /// - 生成処理は高速ですがファイルサイズが巨大なため、リアルタイム用途には不向きです。
    /// flac: 可逆圧縮 (audio/flac)
    /// - 音質を劣化させることなく WAV よりもファイルサイズを小さくできます。
    /// - 生成処理が高速で、ファイルサイズや転送時間も WAV よりは抑えられます。
    /// mp3: MP3 コーデック (audio/mpeg)
    /// - 汎用性が最も高く、すべてのブラウザや OS で対応しています。
    /// - 生成処理が比較的高速で、低遅延なストリーミング再生が可能です。
    /// aac: AAC コーデック + ADTS パケット (audio/aac)
    /// - MP3 よりも圧縮率と音質が良く、多くのブラウザや OS で対応しています。
    /// - ストリーミング再生も可能ですが、MP3 と比較すると若干エンコードが遅めです。
    /// opus: Opus コーデック (audio/ogg; codecs=opus)
    /// - 最高の圧縮効率を誇り、高速・低遅延なストリーミング再生が可能です。
    /// - Apple 製品では長らく公式対応しておらず、iOS Safari では iOS 18.4 以降でのみ対応しています。
    ///
    /// 💡 用途別の推奨形式:
    /// - ブラウザでのリアルタイム再生: mp3 (iOS 対応が不要なら opus)
    /// - ファイルサイズとリアルタイム性を最重視: opus (iOS 対応が必須なら aac)
    /// - 無劣化・最高音質で再生: flac (互換性が求められる場合は wav)
    /// - 未指定時は、デフォルトで MP3 形式で出力します。
    /// </summary>
    [JsonIgnore]
    public string OutputFormat
    {
        get => _outputFormat;
        set
        {
            switch (value.ToLower())
            {
                case "mp3":
                case "aac":
                    break;
                case "wav":
                case "flac":
                    OutputBitrate = null; // ビットレート指定を無効化
                    break;
                case "opus":
                    break;
                default:
                    throw new ArgumentException("Invalid output format. Supported formats are: wav, flac, mp3, aac, opus.", nameof(value));
            }
            _outputFormat = value.ToLower();
            OutputSamplingRate = _outputSamplingRate; // サンプリングレート制限を再適用
        }
    }

    [JsonPropertyName("output_format")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private string? _outputFormatRaw
    {
        get => OutputFormat != DEFAULT_OUTPUT_FORMAT ? OutputFormat : null;
        set => OutputFormat = value ?? DEFAULT_OUTPUT_FORMAT;
    }

    private int? _outputBitrate = null;
    /// <summary>
    /// 音声データの出力ビットレートを kbps 単位の数値で指定します。(例: 192 (kbps))
    /// output_format が wav または flac のときは設定できません（wav は無圧縮、flac は可逆圧縮のため）。
    /// 
    /// 💡 各コーデックの推奨ビットレート:
    /// 
    /// mp3: 128-320kbps (192kbps を推奨)
    /// aac: 96-256kbps (128-192kbps を推奨、MP3 より高効率)
    /// opus: 64-192kbps (128kbps でも MP3 192kbps 相当の音質)
    /// </summary>
    [JsonPropertyName("output_bitrate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? OutputBitrate
    {
        get => _outputBitrate;
        set
        {
            if (value is null)
            {
                _outputBitrate = null;
                return;
            }

            if (OutputFormat == "wav" || OutputFormat == "flac")
            {
                throw new InvalidOperationException("OutputBitrate cannot be set when output_format is wav or flac.");
            }

            if (value < 8 || value > 320)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "OutputBitrate must be between 8 and 320 kbps.");
            }
            _outputBitrate = value;
        }
    }

    /// <summary>
    /// サンプリングレートのデフォルト値: 44100
    /// </summary>
    public const int DEFAULT_OUTPUT_SAMPLING_RATE = 44100;
    private int _outputSamplingRate = DEFAULT_OUTPUT_SAMPLING_RATE;
    /// <summary>
    /// 音声データの出力サンプリングレートを Hz 単位の数値で指定します。(例: 44100 (Hz))
    /// 44100Hz 以上にサンプリングレートを上げても音質は向上しません。
    /// 未指定時は、デフォルトの 44100Hz で出力します。
    /// 
    /// ⚠️ Opus コーデックの仕様上、output_format が opus のとき、output_sampling_rate は 8000/12000/16000/24000/48000Hz のみ指定可能です。
    /// サポートされていないサンプリングレートが指定された場合、指定値以上で最も近い対応サンプリングレートに自動調整します。
    /// </summary>
    [JsonIgnore]
    public int OutputSamplingRate
    {
        get => _outputSamplingRate;
        set
        {
            if (OutputFormat == "opus")
            {
                int[] supportedRates = { 8000, 12000, 16000, 24000, 48000 };
                if (Array.IndexOf(supportedRates, value) == -1)
                {
                    // 指定値以上で最も近い対応サンプリングレートに自動調整
                    foreach (var rate in supportedRates)
                    {
                        if (rate >= value)
                        {
                            _outputSamplingRate = rate;
                            return;
                        }
                    }
                    // 指定値より大きいサンプリングレートがない場合、最大値に設定
                    _outputSamplingRate = supportedRates[^1];
                    return;
                }
            }
            _outputSamplingRate = value;
        }
    }

    [JsonPropertyName("output_sampling_rate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private int? _outputSamplingRateRaw
    {
        get => OutputSamplingRate != DEFAULT_OUTPUT_SAMPLING_RATE ? OutputSamplingRate : null;
        set => OutputSamplingRate = value ?? DEFAULT_OUTPUT_SAMPLING_RATE;
    }

    /// <summary>
    /// チャンネル数のデフォルト値: mono
    /// </summary>
    public const string DEFAULT_OUTPUT_AUDIO_CHANNELS = "mono";
    private string _outputAudioChannels = DEFAULT_OUTPUT_AUDIO_CHANNELS;
    /// <summary>
    /// 音声データのチャンネル数を指定します。mono または stereo を指定します。
    /// stereo を指定すると、モノラル音声（1チャンネル）を複製してステレオ音声（2チャンネル）に変換してから出力します。
    /// 未指定時は、モノラル音声（1チャンネル）で出力します。
    /// </summary>
    [JsonIgnore]
    public string OutputAudioChannels
    {
        get => _outputAudioChannels;
        set
        {
            switch (value.ToLower())
            {
                case "mono":
                case "stereo":
                    _outputAudioChannels = value.ToLower();
                    break;
                default:
                    throw new ArgumentException("Invalid output audio channels. Supported channels are: mono, stereo.", nameof(value));
            }
        }
    }

    [JsonPropertyName("output_audio_channels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonInclude]
    private string? _outputAudioChannelsRaw
    {
        get => OutputAudioChannels != DEFAULT_OUTPUT_AUDIO_CHANNELS ? OutputAudioChannels : null;
        set => OutputAudioChannels = value ?? DEFAULT_OUTPUT_AUDIO_CHANNELS;
    }

    /// <summary>
    /// TTSRequest クラスの新しいインスタンスを初期化します。
    /// </summary>
    public TTSRequest(string modelUuid, string text)
    {
        ModelUuid = modelUuid;
        Text = text;
    }
}