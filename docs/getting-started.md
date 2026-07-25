# Getting Started

## 対応バージョン

- `.NET 6.0`
- `.NET 7.0`
- `.NET 8.0`
- `.NET 9.0`
- `.NET 10.0`

## パッケージの追加

```bash
dotnet add package Aivis.Net
```

## API キーの取得

Aivis Cloud API の利用には API キーが必要です。
[Aivis Project のダッシュボード](https://hub.aivis-project.com/cloud-api/api-keys)で発行してください。

> [!WARNING]
> API キーは課金に直結する認証情報です。ソースコードに直接書かず、環境変数や
> [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) から読み込んでください。

```csharp
var apiKey = Environment.GetEnvironmentVariable("AIVIS_API_KEY")
    ?? throw new InvalidOperationException("AIVIS_API_KEY が設定されていません。");
```

## モデル UUID の取得

音声合成には、使用する音声合成モデルの UUID が必要です。
[AivisHub](https://hub.aivis-project.com/) でモデルを探すか、[モデル検索 API](model-search.md) で取得できます。

## 基本的な使い方

### 音声データを取得する

```csharp
using Aivis;

var options = new AivisClientOptions(apiKey);
var client = new AivisTTSClient(options);

// byte[] として取得する
byte[] audio = await client.SynthesizeAsync(modelUuid, "こんにちは、世界！");
await File.WriteAllBytesAsync("output.mp3", audio);
```

### ストリームとして取得する

音声データはチャンク転送で返るため、`Stream` として受け取ると
合成の完了を待たずに再生や書き込みを始められます。

```csharp
using var audioStream = await client.SynthesizeStreamAsync(modelUuid, "こんにちは、世界！");

using var file = File.Create("output.mp3");
await audioStream.CopyToAsync(file);
```

### 課金情報とあわせて取得する

`SynthesizeWithContentsAsync` はレスポンスヘッダーの課金メタデータを含む `TTSContents` を返します。
クレジット残高の管理やレート制限の監視に利用できます。

```csharp
var contents = await client.SynthesizeWithContentsAsync(modelUuid, "こんにちは、世界！");

Console.WriteLine($"課金モード      : {contents.BillingMode}");
Console.WriteLine($"文字数          : {contents.CharacterCount}");

if (contents.IsPayAsYouGo)
{
    Console.WriteLine($"使用クレジット  : {contents.CreditsUsed}");
    Console.WriteLine($"残りクレジット  : {contents.CreditsRemaining}");
}

using var audioStream = contents.AudioStream;
```

`TTSContents` のメンバーは次のとおりです。

| メンバー | 内容 |
|---|---|
| `AudioStream` | 音声データのストリーム |
| `ContentDisposition` | レスポンスから抽出したファイル名 |
| `BillingMode` | `PayAsYouGo` または `Subscription` |
| `CharacterCount` | 消費された文字数 |
| `CreditsUsed` / `CreditsRemaining` | 使用・残クレジット（従量課金時） |
| `RateLimitRemaining` | 残りリクエスト数（定額プラン時） |
| `IsPayAsYouGo` / `IsSubscription` | 課金モードの判定 |

> [!NOTE]
> `RateLimitRemaining` は現在の実装ではレスポンスヘッダー名が一致しておらず、常に `0` になります。

## リアルタイムストリーミング再生

同梱の `MP3Speaker` を使うと、合成しながらそのまま再生できます。

> [!IMPORTANT]
> `MP3Speaker` は FFmpeg と OpenAL を必要とします。
> インストール方法は[音声再生](audio-playback.md)を参照してください。

```csharp
using Aivis;

var options = new AivisClientOptions(apiKey);
var client = new AivisTTSClient(options);

using var audioStream = await client.SynthesizeStreamAsync(modelUuid, "こんにちは、世界！");

using Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(audioStream);
```

`MP3Speaker` は `IDisposable` です。FFmpeg のプロセスと OpenAL のリソースを解放するため、
`using` で破棄してください。

Windows のみを対象とする場合は、FFmpeg / OpenAL のインストールが不要な
NAudio による再生も利用できます。実装例は
[sample/NAudio/NAudioSpeaker.cs](https://github.com/Atoyr/Aivis.NET/blob/main/sample/NAudio/NAudioSpeaker.cs) にあります。

```csharp
using var audioStream = await client.SynthesizeStreamAsync(modelUuid, "こんにちは、世界！");

Aivis.Speakers.ISpeaker speaker = new Aivis.Sample.NAudio.NAudioSpeaker();
await speaker.PlayAsync(audioStream);
```

## 読み上げ方をカスタマイズする

`TalkToSpeechOptions` で話速・ピッチ・音量・出力形式などを指定できます。
`SetXxx()` メソッドは自身を返すため、続けて記述できます。

```csharp
var ttsOptions = new TalkToSpeechOptions()
    .SetStyle("上機嫌")           // スタイル名で指定
    .SetSpeakingRate(1.2)         // 1.2 倍速
    .SetVolume(0.8)               // 音量 0.8
    .SetOutputFormat(MediaType.MP3)
    .SetOutputBitrate(192);

using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text, ttsOptions);
```

指定できる項目の一覧と制約は [TTS オプション](tts-options.md) を参照してください。

## エラーハンドリング

API がエラーを返した場合、`AivisTTSClient` は状況に応じた例外を投げます。

```csharp
try
{
    using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text);
    // ...
}
catch (UnauthorizedAccessException)
{
    // 401: API キーが無効
}
catch (NotSupportedException ex)
{
    // 402/404/422/429/5xx: 残高不足・モデル未検出・パラメータ不正・レート制限・サーバーエラー
    Console.Error.WriteLine(ex.Message);
}
```

ステータスコードと例外の対応は[トラブルシューティング](troubleshooting.md)にまとめています。

## HttpClient を差し替える

既定では、ライブラリ内部で生成した `HttpClient` を使います。
タイムアウトやリトライ、プロキシの設定を行いたい場合は `IHttpClientProvider` を実装して差し込みます。

```csharp
public sealed class MyHttpClientProvider : IHttpClientProvider
{
    private readonly HttpClient _client = new()
    {
        Timeout = TimeSpan.FromMinutes(5),
    };

    public HttpClient Instance => _client;
}

var options = new AivisClientOptions(apiKey)
{
    HttpClientProvider = new MyHttpClientProvider(),
};
```

## サンプルアプリ

実行できるサンプルがリポジトリに含まれています。

| サンプル | 内容 |
|---|---|
| [StreamPlay](https://github.com/Atoyr/Aivis.NET/tree/main/sample/StreamPlay) | ストリーミング再生 |
| [Contents](https://github.com/Atoyr/Aivis.NET/tree/main/sample/Contents) | 音声データと課金情報の取得 |
| [SearchModels](https://github.com/Atoyr/Aivis.NET/tree/main/sample/SearchModels) | 音声合成モデルの検索 |
| [Users](https://github.com/Atoyr/Aivis.NET/tree/main/sample/Users) | ユーザー情報の取得 |
| [NAudio](https://github.com/Atoyr/Aivis.NET/tree/main/sample/NAudio) | NAudio による再生（Windows） |

```bash
dotnet run -c Release --project sample/StreamPlay/Aivis.Sample.StreamPlay.csproj
```

## 依存関係

- OpenTK 4.9.4（音声再生用）
- FFmpeg / OpenAL（`MP3Speaker` を使う場合のみ。[音声再生](audio-playback.md)を参照）

## リンク

- [GitHubリポジトリ](https://github.com/Atoyr/Aivis.NET)
- [Aivis Project](https://aivis-project.com)
- [Aivis Cloud API リファレンス](https://api.aivis-project.com/v1/docs)
