using Aivis.Schemas;

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

    private int _styleId = TTSRequest.DEFAULT_STYLE_ID;
    /// <summary>
    /// 音声合成モデルの話者のスタイル ID を 0 ~ 31 の範囲で指定します。通常、ノーマルスタイルの ID は 0 です。
    /// スタイル ID には、AivmManifest.speakers[].styles[].local_id の値を指定します。 
    ///
    /// StyleName とは併用できません。 StyleIdを設定した場合、StyleNameは自動的にリセットされます。
    /// 
    /// 当該モデル → 話者 → スタイルに存在しないスタイル ID を指定すると 422 エラーが発生します。
    /// 未指定時は、当該話者のノーマルスタイルを音声合成に利用します。
    /// </summary>
    public int StyleId
    {
        get => _styleId;
        set
        {
            if (0 <= value && value <= 31)
            {
                _styleId = value;
                StyleName = TTSRequest.DEFAULT_STYLE_NAME;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(value), "StyleId must be between 0 and 31.");
            }
        }
    }

    private string _styleName = TTSRequest.DEFAULT_STYLE_NAME;
    /// <summary>
    /// 音声合成モデルの話者のスタイル名を指定します。通常、ノーマルスタイルの名前は「ノーマル」です。
    /// スタイル名には、AivmManifest.speakers[].styles[].name の値を指定します。 
    ///
    /// StyleId とは併用できません。 StyleNameを設定した場合、StyleIdは自動的にリセットされます。
    /// 
    /// 当該モデル → 話者 → スタイルに存在しないスタイル名を指定すると 422 エラーが発生します。
    /// 未指定時は、当該話者のデフォルトスタイルを音声合成に利用します。
    /// </summary>
    public string StyleName
    {
        get
        {
            return _styleName;
        }
        set
        {
            // デフォルト値以外はStyleIdをデフォルト値に変更する
            if (value != TTSRequest.DEFAULT_STYLE_NAME)
            {
                StyleId = TTSRequest.DEFAULT_STYLE_ID;
                _styleName = value;
                return;
            }

            _styleName = TTSRequest.DEFAULT_STYLE_NAME;
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
    /// テキスト読み上げ時の言語を BCP 47 言語タグで指定します。
    /// 2025/08時点では日本語のみ対応しています。
    /// </summary>
    public string Language { get; set; } = TTSRequest.DEFAULT_LANGUAGE;

    private double _speakingRate = TTSRequest.DEFAULT_SPEAKING_RATE;
    /// <summary>
    /// 話す速さを 0.5 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 2.0 で 2 倍速、0.5 で 0.5 倍速になります。
    /// </summary>
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

    private double emotionIntensity = TTSRequest.DEFAULT_EMOTION_INTENSITY;
    /// <summary>
    /// 選択した話者スタイルの感情表現の強弱を 0.0 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 数値が大きいほど、選択した話者スタイルに近い感情表現が込められた声になります。
    /// 例えば話者スタイルが「上機嫌」なら、数値が大きいほどより嬉しそうな明るい話し方になります。
    /// 
    /// 一方で、話者やスタイルによっては、数値を上げすぎると発声がおかしくなったり、棒読みで不自然な声になる場合もあります。
    /// 正しく発声できる上限値は話者やスタイルごとに異なります。必要に応じて最適な値を見つけて調整してください。
    /// 
    /// ⚠️ ノーマルスタイルを利用する場合、EmotionalIntensity (話者スタイルの感情表現の強さ) は指定しても効果がありません。
    /// ノーマルスタイルは全スタイルの平均的な特徴を持つため、感情表現の強さは自動で最適化されます。
    /// </summary>
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

    private double tempoDynamics = TTSRequest.DEFAULT_TEMPO_DYNAMICS;
    /// <summary>
    /// 話す速さの緩急の強弱を 0.0 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 数値が大きいほど、より早口で生っぽい抑揚がついた声になります。
    /// 声の表現を細かく変化させたい際に調整してみてください。
    /// </summary>
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

    private double pitch = TTSRequest.DEFAULT_PITCH;
    /// <summary>
    /// 声のピッチの高さ（音高）を -1.0 ~ 1.0 の範囲で指定します。(デフォルト: 0.0)
    /// 数値が大きいほど、より高いピッチの声になります。
    /// 
    /// ⚠️ 仕様上、この値を 0.0 から変更すると、合成音声の品質が劣化する場合があります。また、生成速度が大幅に低下します。
    /// クリアで高品質な音声を生成したいケースではなるべく指定しないことをおすすめします。
    /// </summary>
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

    private double _volume = TTSRequest.DEFAULT_VOLUME;
    /// <summary>
    /// 全体の音量の大きさを 0.0 ~ 2.0 の範囲で指定します。(デフォルト: 1.0)
    /// 数値が大きいほど、より大きな声になります。
    /// </summary>
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

    private double _leadingSilenceSeconds = TTSRequest.DEFAULT_LEADING_SILENCE_SECONDS;
    /// <summary>
    /// 音声先頭の無音時間の長さを秒単位で指定します。(デフォルト: 0.1)
    /// 0.0 を指定すると、音声先頭の無音時間を完全に削除できます。
    /// ストリーミング再生時に 0.0 を指定すると、再生開始までの待機時間をさらに削減できます。
    /// </summary>
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

    private double _trailingSilenceSeconds = TTSRequest.DEFAULT_TRAILING_SILENCE_SECONDS;
    /// <summary>
    /// 音声末尾の無音時間の長さを秒単位で指定します。(デフォルト: 0.1)
    /// 0.0 を指定すると、音声末尾の無音時間を完全に削除できます。
    /// </summary>
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

    private double _lineBreakSilenceSeconds = TTSRequest.DEFAULT_LINE_BREAK_SILENCE_SECONDS;
    /// <summary>
    /// テキストの改行ごとに挟む無音区間の長さを秒単位で指定します。(デフォルト: 0.4)
    /// 0.0 を指定すると、改行間の無音時間を完全に削除できます。
    /// 
    /// ⚠️ SSML が有効かつ &lt;p&gt;, &lt;s&gt; が記述されている場合、line_break_silence_seconds の値は適用されません。
    /// 必要に応じて &lt;break time="..."&gt; タグで無音区間をさらに調整可能です。
    /// </summary>
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

    private MediaType _outputFormat = MediaType.MP3;
    /// <summary>
    /// 音声データの出力形式を指定します。用途に応じて最適な形式を選択してください。
    /// wav, flac, mp3, aac, opus
    /// </summary>
    public MediaType OutputFormat
    {
        get => _outputFormat;
        set
        {
            switch (value)
            {
                case MediaType.MP3:
                case MediaType.AAC:
                    break;
                case MediaType.WAV:
                case MediaType.FLAC:
                    OutputBitrate = null; // ビットレート指定を無効化
                    break;
                case MediaType.OPUS:
                    break;
                default:
                    throw new ArgumentException("Invalid output format. Supported formats are: wav, flac, mp3, aac, opus.", nameof(value));
            }
            _outputFormat = value;
            OutputSamplingRate = _outputSamplingRate; // サンプリングレート制限を再適用
        }
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

            if (OutputFormat == MediaType.WAV || OutputFormat == MediaType.FLAC)
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

    private int _outputSamplingRate = TTSRequest.DEFAULT_OUTPUT_SAMPLING_RATE;
    /// <summary>
    /// 音声データの出力サンプリングレートを Hz 単位の数値で指定します。(例: 44100 (Hz))
    /// 44100Hz 以上にサンプリングレートを上げても音質は向上しません。
    /// 未指定時は、デフォルトの 44100Hz で出力します。
    /// 
    /// ⚠️ Opus コーデックの仕様上、output_format が opus のとき、output_sampling_rate は 8000/12000/16000/24000/48000Hz のみ指定可能です。
    /// サポートされていないサンプリングレートが指定された場合、指定値以上で最も近い対応サンプリングレートに自動調整します。
    /// </summary>
    public int OutputSamplingRate
    {
        get => _outputSamplingRate;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "OutputSamplingRate must be greater than 0.");
            }

            if (OutputFormat == MediaType.OPUS)
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

    /// <summary>
    /// 音声データのチャンネル数を指定します。mono または stereo を指定します。
    /// stereo を指定すると、モノラル音声（1チャンネル）を複製してステレオ音声（2チャンネル）に変換してから出力します。
    /// 未指定時は、モノラル音声（1チャンネル）で出力します。
    /// </summary>
    public AudioChannel OutputAudioChannels { get; set; } = AudioChannel.Mono;

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
    /// 話者スタイルを設定します。
    /// </summary>
    /// <param name="styleName">話者スタイル</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetStyle(string styleName)
    {
        StyleName = styleName;
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
    /// テキスト読み上げ時の言語を設定します。
    /// </summary>
    /// <param name="language">BCP 47言語タグ（例: "ja-JP"）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetLanguage(string language)
    {
        Language = language;
        return this;
    }

    /// <summary>
    /// 話す速さを設定します。
    /// </summary>
    /// <param name="speakingRate">話す速さ（0.5〜2.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetSpeakingRate(double speakingRate)
    {
        SpeakingRate = speakingRate;
        return this;
    }

    /// <summary>
    /// 選択した話者スタイルの感情表現の強弱を設定します。
    /// </summary>
    /// <param name="emotionIntensity">感情表現の強弱（0.0〜2.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetEmotionIntensity(double emotionIntensity)
    {
        EmotionIntensity = emotionIntensity;
        return this;
    }

    /// <summary>
    /// 話す速さの緩急の強弱を設定します。
    /// </summary>
    /// <param name="tempoDynamics">話す速さの緩急の強弱（0.0〜2.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetTempoDynamics(double tempoDynamics)
    {
        TempoDynamics = tempoDynamics;
        return this;
    }

    /// <summary>
    /// 声のピッチの高さ（音高）を設定します。
    /// </summary>
    /// <param name="pitch">ピッチの高さ（-1.0〜1.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetPitch(double pitch)
    {
        Pitch = pitch;
        return this;
    }

    /// <summary>
    /// 全体の音量の大きさを設定します。
    /// </summary>
    /// <param name="volume">音量の大きさ（0.0〜2.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetVolume(double volume)
    {
        Volume = volume;
        return this;
    }

    /// <summary>
    /// 音声先頭の無音時間の長さを秒単位で設定します。
    /// </summary>
    /// <param name="leadingSilenceSeconds">音声先頭の無音時間（0.0〜60.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetLeadingSilenceSeconds(double leadingSilenceSeconds)
    {
        LeadingSilenceSeconds = leadingSilenceSeconds;
        return this;
    }

    /// <summary>
    /// 音声末尾の無音時間の長さを秒単位で設定します。
    /// </summary>
    /// <param name="trailingSilenceSeconds">音声末尾の無音時間（0.0〜60.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetTrailingSilenceSeconds(double trailingSilenceSeconds)
    {
        TrailingSilenceSeconds = trailingSilenceSeconds;
        return this;
    }

    /// <summary>
    /// テキストの改行ごとに挟む無音区間の長さを秒単位で設定します。
    /// </summary>
    /// <param name="lineBreakSilenceSeconds">改行間の無音時間（0.0〜60.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetLineBreakSilenceSeconds(double lineBreakSilenceSeconds)
    {
        LineBreakSilenceSeconds = lineBreakSilenceSeconds;
        return this;
    }

    /// <summary>
    /// 音声の先頭と末尾の無音時間を同時に設定します。
    /// </summary>
    /// <param name="leadingSilenceSeconds">音声先頭の無音時間（0.0〜60.0の範囲）</param>
    /// <param name="trailingSilenceSeconds">音声末尾の無音時間（0.0〜60.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetSilenceSeconds(double leadingSilenceSeconds, double trailingSilenceSeconds)
    {
        LeadingSilenceSeconds = leadingSilenceSeconds;
        TrailingSilenceSeconds = trailingSilenceSeconds;
        return this;
    }

    /// <summary>
    /// 音声の先頭、末尾、および改行間の無音時間を同時に設定します。
    /// </summary>
    /// <param name="leadingSilenceSeconds">音声先頭の無音時間（0.0〜60.0の範囲）</param>
    /// <param name="trailingSilenceSeconds">音声末尾の無音時間（0.0〜60.0の範囲）</param>
    /// <param name="lineBreakSilenceSeconds">改行間の無音時間（0.0〜60.0の範囲）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetSilenceSeconds(double leadingSilenceSeconds, double trailingSilenceSeconds, double lineBreakSilenceSeconds)
    {
        SetSilenceSeconds(leadingSilenceSeconds, trailingSilenceSeconds);
        LineBreakSilenceSeconds = lineBreakSilenceSeconds;
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

    /// <summary>
    /// 出力するファイルフォーマットを設定します。
    /// </summary>
    /// <param name="format">出力フォーマット</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetOutputFormat(string format)
    {
        switch (format.ToLowerInvariant())
        {
            case "wav":
                return SetOutputFormat(MediaType.WAV);
            case "flac":
                return SetOutputFormat(MediaType.FLAC);
            case "mp3":
                return SetOutputFormat(MediaType.MP3);
            case "aac":
                return SetOutputFormat(MediaType.AAC);
            case "opus":
                return SetOutputFormat(MediaType.OPUS);
            default:
                throw new ArgumentException("Invalid format. Supported formats are: wav, flac, mp3, aac, opus.", nameof(format));
        }
    }

    /// <summary>
    /// 音声データの出力ビットレートをkbps単位で設定します。
    /// </summary>
    /// <param name="outputBitrate">出力ビットレート（8〜320kbpsの範囲、nullの場合はデフォルト値を使用）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetOutputBitrate(int? outputBitrate)
    {
        OutputBitrate = outputBitrate;
        return this;
    }

    /// <summary>
    /// 音声データの出力サンプリングレートをHz単位で設定します。
    /// </summary>
    /// <param name="outputSamplingRate">出力サンプリングレート（Hz単位）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetOutputSamplingRate(int outputSamplingRate)
    {
        OutputSamplingRate = outputSamplingRate;
        return this;
    }

    /// <summary>
    /// 音声データのチャンネル数を設定します。
    /// </summary>
    /// <param name="outputAudioChannels">出力チャンネル数</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetOutputAudioChannels(AudioChannel outputAudioChannels)
    {
        OutputAudioChannels = outputAudioChannels;
        return this;
    }

    /// <summary>
    /// 音声データのチャンネル数を文字列で設定します。
    /// </summary>
    /// <param name="outputAudioChannels">出力チャンネル数（"mono" または "stereo"）</param>
    /// <returns>TalkToSpeechOptionsのインスタンス</returns>
    public TalkToSpeechOptions SetOutputAudioChannels(string outputAudioChannels)
    {
        switch (outputAudioChannels.ToLowerInvariant())
        {
            case "mono":
                return SetOutputAudioChannels(AudioChannel.Mono);
            case "stereo":
                return SetOutputAudioChannels(AudioChannel.Stereo);
            default:
                throw new ArgumentException("Invalid outputAudioChannels. Supported values are: mono, stereo.", nameof(outputAudioChannels));
        }
    }
}