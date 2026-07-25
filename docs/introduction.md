# Introduction

Aivis APIの非公式C#ライブラリです。Text-to-Speech（音声合成）機能を提供します。

## 対応バージョン

- `.NET 6.0`
- `.NET 7.0`
- `.NET 8.0`
- `.NET 9.0`
- `.NET 10.0`

## パッケージの追加

``` 
dotnet add package Aivis.Net
```

## 使用例

Text-to-Speech (ストリーミング再生)
> [!NOTE]
> FFmpegとOpenALがインストールされている必要があります。
> インストール方法は[音声再生](audio-playback.md)を参照してください。
``` C#
using Aivis;

// APIキーでクライアントを初期化
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// テキストを音声に変換
using var audioStream = await client.SynthesizeStreamAsync("model-uuid", "こんにちは、世界！");

// 音声を再生
using Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(audioStream);
```

## 機能

- ✅ Aivis Cloud APIを使用したテキスト音声合成（`AivisTTSClient`）
- ✅ 音声合成モデルの検索・詳細取得・ダウンロードURL取得（`AivisModelClient`）
- ✅ ユーザーアカウント情報の取得（`AivisUsersClient`）
- ✅ 読み上げパラメータの細かな指定（話速・ピッチ・音量・感情表現・出力形式など）
- ✅ FFmpegとOpenALによる音声再生サポート（`MP3Speaker`）
- ✅ レスポンスメタデータ（課金情報、文字数、レート制限など）
- ✅ .NET 6.0 ~ .NET 10.0 をサポート
- ✅ async/awaitパターン

## 次に読むもの

| ページ | 内容 |
|---|---|
| [Getting Started](getting-started.md) | 導入手順と基本的な使い方 |
| [TTS オプション](tts-options.md) | 読み上げパラメータの一覧と指定方法 |
| [モデル検索](model-search.md) | 音声合成モデルを検索・取得する |
| [ユーザー API](users.md) | アカウント情報を取得する |
| [音声再生](audio-playback.md) | 再生環境の準備と `ISpeaker` の実装 |
| [トラブルシューティング](troubleshooting.md) | よくあるエラーと対処法 |
| [アーキテクチャ](architecture.md) | 内部構造と拡張ポイント |
