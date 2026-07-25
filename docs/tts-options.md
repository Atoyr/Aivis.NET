# TTS オプション

`TalkToSpeechOptions` は、音声合成の挙動を細かく制御するためのクラスです。
`AivisTTSClient` の各 `SynthesizeXxxAsync` メソッドの第 3 引数に渡します。

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetSpeakingRate(1.2)
    .SetVolume(0.8);

using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text, ttsOptions);
```

プロパティに直接代入することもできます。`SetXxx()` メソッドは自身を返すため、続けて記述できます。

```csharp
var ttsOptions = new TalkToSpeechOptions
{
    SpeakingRate = 1.2,
    Volume = 0.8,
};
```

> [!IMPORTANT]
> 範囲外の値を設定すると、API を呼び出す前に `ArgumentOutOfRangeException` が発生します。
> 利用者の入力をそのまま渡す場合は、あらかじめ検証するか例外を捕捉してください。

## 話者とスタイル

| プロパティ | 型 | 既定値 | 説明 |
|---|---|---|---|
| `SpeakerUuid` | `Guid?` | `null` | マルチスピーカーモデルで話者を指定します。単一話者モデルでは不要です |
| `StyleId` | `int` | `0` | スタイル ID（0〜31）。通常、ノーマルスタイルは `0` です |
| `StyleName` | `string` | `ノーマル` | スタイル名。`AivmManifest.speakers[].styles[].name` の値です |

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetSpeakerUuid("00000000-0000-0000-0000-000000000000")
    .SetStyle("上機嫌");
```

> [!WARNING]
> `StyleId` と `StyleName` は併用できません。
> 一方を設定すると、もう一方は自動的に既定値へリセットされます。
> 後から設定したほうが有効になります。

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetStyle(3)          // StyleId = 3
    .SetStyle("上機嫌");  // StyleName が有効になり、StyleId は 0 に戻る
```

指定できるスタイルはモデルごとに異なります。
[モデル詳細の取得](model-search.md#モデルの詳細を取得する)で確認できます。

## テキストの解釈

| プロパティ | 型 | 既定値 | 説明 |
|---|---|---|---|
| `UseSsml` | `bool` | `true` | SSML のサブセットを解釈します |
| `Language` | `string` | `ja` | BCP 47 言語タグ。現在は日本語のみ対応です |
| `UserDictionaryUuid` | `Guid?` | `null` | 適用するユーザー辞書。事前にユーザー辞書 API での作成が必要です |

対応している SSML タグは次のサブセットです。

- `<break time="..." />` / `<break strength="..." />` — 無音の挿入
- `<prosody rate="..." pitch="..." volume="...">` — 部分的な調整
- `<sub alias="...">` — 読みの置き換え
- `<p>` / `<s>` — 段落・文の区切り

```csharp
var text = "こんにちは。<break time=\"500ms\"/>本日は<sub alias=\"あいびす\">Aivis</sub>のご紹介です。";
using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text);
```

> [!CAUTION]
> `UseSsml` は既定で有効です。利用者の入力をそのまま渡すと `<` や `&` が
> タグとして解釈され、意図しない読み上げや 422 エラーの原因になります。
> プレーンテキストとして扱う場合は `SetUseSsml(false)` を指定してください。

```csharp
var ttsOptions = new TalkToSpeechOptions().SetUseSsml(false);
```

> [!NOTE]
> `<p>` / `<s>` を使うと `LineBreakSilenceSeconds` の設定は適用されません。
> 無音の長さは `<break time="...">` で調整してください。

## 声の調整

| プロパティ | 型 | 範囲 | 既定値 | 説明 |
|---|---|---|---|---|
| `SpeakingRate` | `double` | 0.5 〜 2.0 | `1.0` | 話す速さ。2.0 で 2 倍速 |
| `EmotionIntensity` | `double` | 0.0 〜 2.0 | `1.0` | 選択したスタイルの感情表現の強さ |
| `TempoDynamics` | `double` | 0.0 〜 2.0 | `1.0` | 話す速さの緩急。大きいほど生っぽい抑揚になります |
| `Pitch` | `double` | -1.0 〜 1.0 | `0.0` | 声の高さ |
| `Volume` | `double` | 0.0 〜 2.0 | `1.0` | 全体の音量 |

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetSpeakingRate(1.1)
    .SetEmotionIntensity(1.3)
    .SetTempoDynamics(1.2);
```

> [!NOTE]
> ノーマルスタイルでは `EmotionIntensity` は効果がありません。
> ノーマルスタイルは全スタイルの平均的な特徴を持ち、感情表現の強さが自動で最適化されるためです。

> [!WARNING]
> `Pitch` を `0.0` 以外にすると、合成音声の品質が劣化する場合があり、生成速度も大幅に低下します。
> クリアで高品質な音声が必要な場面では変更しないことをおすすめします。

話者やスタイルによっては、`EmotionIntensity` を上げすぎると発声が不自然になります。
正しく発声できる上限はモデルごとに異なるため、実際に聴きながら調整してください。

## 無音時間

| プロパティ | 型 | 範囲 | 既定値 | 説明 |
|---|---|---|---|---|
| `LeadingSilenceSeconds` | `double` | 0.0 〜 60.0 | `0.1` | 音声先頭の無音時間（秒） |
| `TrailingSilenceSeconds` | `double` | 0.0 〜 60.0 | `0.1` | 音声末尾の無音時間（秒） |
| `LineBreakSilenceSeconds` | `double` | 0.0 〜 60.0 | `0.4` | 改行ごとに挟む無音時間（秒） |

まとめて設定するメソッドも用意されています。

```csharp
// 先頭・末尾
var ttsOptions = new TalkToSpeechOptions().SetSilenceSeconds(0.0, 0.0);

// 先頭・末尾・改行
var ttsOptions2 = new TalkToSpeechOptions().SetSilenceSeconds(0.0, 0.0, 0.2);
```

> [!TIP]
> ストリーミング再生では `LeadingSilenceSeconds` を `0.0` にすると、
> 再生が始まるまでの待ち時間をさらに短縮できます。

## 出力形式

| プロパティ | 型 | 既定値 | 説明 |
|---|---|---|---|
| `OutputFormat` | `MediaType` | `MP3` | `WAV` / `FLAC` / `MP3` / `AAC` / `OPUS` |
| `OutputBitrate` | `int?` | `null` | ビットレート（8〜320 kbps）。`WAV` / `FLAC` では設定不可 |
| `OutputSamplingRate` | `int` | `44100` | サンプリングレート（Hz） |
| `OutputAudioChannels` | `AudioChannel` | `Mono` | `Mono` / `Stereo` |

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetOutputFormat(MediaType.OPUS)
    .SetOutputBitrate(128)
    .SetOutputSamplingRate(48000)
    .SetOutputAudioChannels(AudioChannel.Stereo);
```

文字列でも指定できます。

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetOutputFormat("opus")
    .SetOutputAudioChannels("stereo");
```

### 形式ごとの推奨ビットレート

| 形式 | 推奨 | 備考 |
|---|---|---|
| `MP3` | 128〜320 kbps（192 kbps 推奨） | 互換性が高く、`MP3Speaker` がそのまま再生できます |
| `AAC` | 96〜256 kbps（128〜192 kbps 推奨） | MP3 より高効率 |
| `OPUS` | 64〜192 kbps（128 kbps 推奨） | 128 kbps で MP3 192 kbps 相当の音質 |
| `WAV` | — | 無圧縮 |
| `FLAC` | — | 可逆圧縮 |

### 設定順序に注意

`OutputFormat` の変更は、他のプロパティに波及します。

| 操作 | 副作用 |
|---|---|
| `OutputFormat` を `WAV` / `FLAC` に設定 | `OutputBitrate` が `null` にリセットされます |
| `OutputFormat` を変更 | `OutputSamplingRate` の制約が再適用されます |

```csharp
// ✅ 正しい順序: 形式を決めてからビットレートを指定する
var ok = new TalkToSpeechOptions()
    .SetOutputFormat(MediaType.MP3)
    .SetOutputBitrate(192);

// ❌ ビットレートが破棄される
var ng = new TalkToSpeechOptions()
    .SetOutputBitrate(192)
    .SetOutputFormat(MediaType.WAV);   // ここで OutputBitrate が null になる
```

`WAV` / `FLAC` を指定した状態で `OutputBitrate` を設定すると `InvalidOperationException` が発生します。

### Opus のサンプリングレート

`OutputFormat` が `OPUS` のとき、指定できるサンプリングレートは
8000 / 12000 / 16000 / 24000 / 48000 Hz のみです。
それ以外の値を指定した場合、**例外にはならず**、指定値以上で最も近い対応値に自動調整されます。

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetOutputFormat(MediaType.OPUS)
    .SetOutputSamplingRate(44100);

Console.WriteLine(ttsOptions.OutputSamplingRate);   // 48000
```

> [!NOTE]
> 44100 Hz より高いサンプリングレートを指定しても音質は向上しません。

## 未対応のオプション

Aivis Cloud API には存在しますが、現在のライブラリでは指定できない項目です。

| API のフィールド | 内容 |
|---|---|
| `use_volume_normalizer` | 音量正規化（ラウドネスノーマライズ）。常に API 側の既定値（有効）で動作します |

## 設定例

### ストリーミング再生を最速で始める

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetOutputFormat(MediaType.MP3)
    .SetLeadingSilenceSeconds(0.0);

using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text, ttsOptions);
using Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(audioStream);
```

### 高音質でファイルに保存する

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetOutputFormat(MediaType.FLAC)
    .SetOutputSamplingRate(44100)
    .SetOutputAudioChannels(AudioChannel.Stereo);

byte[] audio = await client.SynthesizeAsync(modelUuid, text, ttsOptions);
await File.WriteAllBytesAsync("output.flac", audio);
```

### 通話品質で帯域を抑える

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetOutputFormat(MediaType.OPUS)
    .SetOutputBitrate(64)
    .SetOutputSamplingRate(24000);
```
