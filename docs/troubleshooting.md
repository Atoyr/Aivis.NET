# トラブルシューティング

よくあるエラーとその対処法をまとめています。

## API エラー

`AivisTTSClient` は、API が返したステータスコードを次の例外に変換します。

| ステータス | 例外 | 意味 | 対処 |
|---|---|---|---|
| 401 | `UnauthorizedAccessException` | API キーが未設定または無効 | [ダッシュボード](https://hub.aivis-project.com/cloud-api/api-keys)でキーを確認する |
| 402 | `NotSupportedException` | クレジット残高が不足 | クレジットを追加する。事前確認は[ユーザー API](users.md#クレジット残高を確認してから合成する)を参照 |
| 404 | `NotSupportedException` | モデルが見つからない | モデル UUID を確認する。[モデル検索](model-search.md)で取得できる |
| 422 | `NotSupportedException` | パラメータが不正 | 話者 UUID・スタイル・SSML の記述を確認する |
| 429 | `NotSupportedException` | レート制限に到達 | 時間をおいて再試行する |
| 500 / 502 / 503 / 504 | `NotSupportedException` | 音声合成サーバー（Citoras）側のエラー | 時間をおいて再試行する |

402 以降はいずれも `NotSupportedException` になるため、区別するにはメッセージを参照します。

```csharp
try
{
    using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text);
}
catch (UnauthorizedAccessException)
{
    // 401
}
catch (NotSupportedException ex)
{
    Console.Error.WriteLine(ex.Message);   // ステータスコードが含まれる
}
```

`AivisModelClient` と `AivisUsersClient` の例外は、[モデル検索](model-search.md#例外)と
[ユーザー API](users.md#例外)のページを参照してください。

### `ArgumentException: APIキーが設定されていません`

`AivisTTSClient` は、コンストラクタで API キーの有無を検証します。

```csharp
// ❌ API キーなしでは生成できない
var client = new AivisTTSClient(new AivisClientOptions());

// ✅
var client = new AivisTTSClient(new AivisClientOptions(apiKey));
```

`AivisModelClient` は API キーなしで利用できます。
`AivisUsersClient` も生成自体は可能で、`GetMe` を呼んだ時点で検証されます。

### 422 が返る

多くはパラメータの組み合わせが原因です。次の点を確認してください。

- **話者 UUID がモデルに存在しない** — [モデル詳細](model-search.md#話者とスタイルを調べる)で
  `Speakers[].AivmSpeakerUuid` を確認する
- **スタイル ID / スタイル名がモデルに存在しない** — `Speakers[].Styles[]` を確認する
- **SSML の記述が不正** — テキストに `<` や `&` が含まれていないか確認する。
  プレーンテキストとして扱うなら `SetUseSsml(false)` を指定する

```csharp
// 利用者の入力をそのまま渡す場合
var ttsOptions = new TalkToSpeechOptions().SetUseSsml(false);
using var audioStream = await client.SynthesizeStreamAsync(modelUuid, userInput, ttsOptions);
```

## オプション設定のトラブル

### `ArgumentOutOfRangeException` が発生する

`TalkToSpeechOptions` と `SearchVoiceModelsOptions` は、値の範囲をプロパティセッターで検証します。
API を呼ぶ前に例外が発生するため、範囲を確認してください。
一覧は [TTS オプション](tts-options.md)にあります。

### 指定したはずのスタイルが反映されない

`StyleId` と `StyleName` は併用できず、一方を設定するともう一方は既定値へリセットされます。
**後から設定したほうが有効**になります。

```csharp
// StyleName が有効。StyleId は 0 に戻っている
var ttsOptions = new TalkToSpeechOptions()
    .SetStyle(3)
    .SetStyle("上機嫌");
```

### 指定したビットレートが無視される

`OutputFormat` を `WAV` / `FLAC` に設定すると `OutputBitrate` は `null` にリセットされます。
形式を先に指定してからビットレートを設定してください。

```csharp
// ❌ ビットレートが破棄される
new TalkToSpeechOptions().SetOutputBitrate(192).SetOutputFormat(MediaType.WAV);

// ✅
new TalkToSpeechOptions().SetOutputFormat(MediaType.MP3).SetOutputBitrate(192);
```

`WAV` / `FLAC` の状態で `OutputBitrate` を設定すると `InvalidOperationException` が発生します。

### 指定したサンプリングレートと違う値になる

`OutputFormat` が `OPUS` のとき、サンプリングレートは
8000 / 12000 / 16000 / 24000 / 48000 Hz のいずれかに自動調整されます（例外にはなりません）。

### `RateLimitRemaining` が常に 0 になる

現在の実装は読み取るヘッダー名が API の仕様と一致していないため、
`TTSContents.RateLimitRemaining` は常に `0` になります。
定額プランのレート制限を監視する用途では、この値を使えません。

### 音量正規化を無効にしたい

API の `use_volume_normalizer` に相当するオプションは未実装です。
現在は常に API 側の既定値（有効）で動作します。

## 音声再生のトラブル

### `Win32Exception` / `ffmpeg起動に失敗しました`

FFmpeg が見つかっていません。

1. `ffmpeg -version` が実行できるか確認する
2. インストールされていない場合は[音声再生](audio-playback.md#ffmpeg-と-openal-のインストール)の手順に従う
3. `PATH` に含まれていない場所にある場合は、パスを明示する

```csharp
using MP3Speaker speaker = new();
speaker.ConfigureBuildOptions(opt => opt.SetFFmpegPath("/usr/local/bin/ffmpeg"));
```

### `InvalidOperationException: OpenAL device を開けません`

OpenAL が利用できない状態です。

- OpenAL がインストールされているか確認する（Linux では `libopenal1`）
- 再生デバイスが認識されているか確認する

```csharp
foreach (var name in Aivis.Speakers.Device.ListPlaybackDevices())
{
    Console.WriteLine(name);
}
```

- WSL・Docker・CI などの環境では、音声デバイスが存在せず再生できない場合があります。
  その環境ではファイルへ保存する運用に切り替えてください

```csharp
byte[] audio = await client.SynthesizeAsync(modelUuid, text);
await File.WriteAllBytesAsync("output.mp3", audio);
```

### 音が出ない・ノイズになる

`MP3Speaker` は MP3 を前提にデコードします。
`OutputFormat` に `MP3` 以外を指定していないか確認してください。

```csharp
// MP3Speaker で再生するなら MP3 のままにする
var ttsOptions = new TalkToSpeechOptions().SetOutputFormat(MediaType.MP3);
```

### 音が途切れる

デコードや転送が再生に追いつかない場合に発生します。バッファを増やしてください。

```csharp
using MP3Speaker speaker = new(bufferCount: 8, bufferMillis: 200);
```

### `InvalidOperationException: すでに再生中です`

1 つの `MP3Speaker` インスタンスでは同時に複数の音声を再生できません。
前の `PlayAsync` の完了を待つか、インスタンスを分けてください。

### 再生後にプロセスが終了しない

`MP3Speaker` を破棄していないと、FFmpeg のプロセスと OpenAL のリソースが残ります。

```csharp
using MP3Speaker speaker = new();   // using で破棄する
```

## ビルド・実行時のトラブル

### `NETSDK1045` などターゲットフレームワークのエラー

Aivis.Net は .NET 6.0 / 7.0 / 8.0 / 9.0 / 10.0 に対応しています。
プロジェクトの `TargetFramework` がこの範囲内か確認してください。

### 再生が始まるまで時間がかかる

- `SynthesizeAsync`（`byte[]` を返す）は合成の完了を待ちます。
  すぐに再生を始めたい場合は `SynthesizeStreamAsync` を使ってください
- `LeadingSilenceSeconds` を `0.0` にすると、先頭の無音分だけ短縮できます

```csharp
var ttsOptions = new TalkToSpeechOptions().SetLeadingSilenceSeconds(0.0);
using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text, ttsOptions);
```

### 長いテキストでタイムアウトする

既定の `HttpClient` のタイムアウトは 100 秒です。
長文を扱う場合は `IHttpClientProvider` を実装して延長してください。

```csharp
public sealed class LongTimeoutProvider : IHttpClientProvider
{
    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromMinutes(10) };
    public HttpClient Instance => _client;
}

var options = new AivisClientOptions(apiKey)
{
    HttpClientProvider = new LongTimeoutProvider(),
};
```

## 解決しない場合

- [Issue](https://github.com/Atoyr/Aivis.NET/issues) で報告・質問してください。
  使い方の質問も歓迎しています
- API そのものの挙動については [Aivis Cloud API リファレンス](https://api.aivis-project.com/v1/docs)を参照してください
- セキュリティに関わる問題は
  [SECURITY.md](https://github.com/Atoyr/Aivis.NET/blob/main/SECURITY.md) の手順に従って非公開で報告してください
