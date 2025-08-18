# Introduction

Aivis APIの非公式C#ライブラリです。Text-to-Speech（音声合成）機能を提供します。

## 対応バージョン

- `.NET 6.0`
- `.NET 7.0`
- `.NET 8.0`
- `.NET 9.0`

## パッケージの追加

``` 
dotnet add package Aivis.Net
```

## 使用例

Text-to-Speech (ストリーミング再生)
> [!NOTE]
> FFmpegとOpenALがインストールされている必要があります。
> インストール方法は[音声再生の利用](getting-started.html#音声再生の利用)を参照してください。
``` C#
using Aivis;

// APIキーでクライアントを初期化
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// テキストを音声に変換
using var audioStream = await client.SynthesizeStreamAsync("model-uuid", "こんにちは、世界！");

// 音声を再生
Aivis.Speakers.MP3Speaker speaker = new ();
await speaker.PlayAsync(audioStream);
speaker.Dispose();
```

## 機能

- ✅ Aivis APIを使用したテキスト音声合成
- ✅ FFmpegとOpenALによる音声再生サポート
- ✅ レスポンスメタデータ（課金情報、文字数、レート制限など）
- ✅ .NET 8.0および.NET 9.0サポート
- ✅ async/awaitパターン
