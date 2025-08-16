# Aivis.NET

Aivis APIの非公式C#ライブラリです。Text-to-Speech（音声合成）機能を提供します。

## インストール

```bash
dotnet add package Aivis.Net
```

## クイックスタート

```csharp
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

## 使用方法

### 基本的なTTS

```csharp
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// 音声ストリームのみを取得
using var audioStream = await client.SynthesizeStreamAsync("model-uuid", "こんにちは、世界！");

// 音声とメタデータを取得
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "ここにテキストを入力");
Console.WriteLine($"文字数: {contents.CharacterCount}");
Console.WriteLine($"使用クレジット: {contents.CreditsUsed}");
```

### 音声再生
> FFmpegとOpenALがインストールされている必要があります。

```csharp
Aivis.Speakers.MP3Speaker speaker = new ();
await speaker.PlayAsync(contents.AudioStream);
speaker.Dispose();
```

## 依存関係

- OpenTK 4.9.4（音声再生用）

## リンク

- [GitHubリポジトリ](https://github.com/Atoyr/Aivis-net)
- [Aivis Project](https://aivis-project.com)

## ライセンス

© 2025 Ryota Uchiyama

---

# Aivis.NET (English)

An unofficial C# library for the Aivis API, providing Text-to-Speech functionality.

## Installation

```bash
dotnet add package Aivis.Net
```

## Quick Start

```csharp
using Aivis;

// Initialize client with your API key
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// Synthesize text to speech
using var audioStream = await client.SynthesizeStreamAsync("model-uuid", "こんにちは、世界！");

// Play audio
Aivis.Speakers.MP3Speaker speaker = new ();
await speaker.PlayAsync(audioStream);
speaker.Dispose();
```

## Features

- ✅ Text-to-Speech synthesis via Aivis API
- ✅ Audio playback support with FFmpeg and OpenAL
- ✅ Response metadata (billing, character count, rate limits)
- ✅ Support for .NET 8.0 and .NET 9.0
- ✅ Async/await pattern

## Usage

### Basic TTS

```csharp
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// Get audio stream only
using var audioStream = await client.SynthesizeStreamAsync("model-uuid", "こんにちは、世界！");

// Get audio with metadata
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "Your text here");
Console.WriteLine($"Character Count: {contents.CharacterCount}");
Console.WriteLine($"Credits Used: {contents.CreditsUsed}");
```

### Audio Playback
> Need install to FFmpeg and OpenAL.
```csharp
Aivis.Speakers.MP3Speaker speaker = new ();
await speaker.PlayAsync(contents.AudioStream);
speaker.Dispose();
```

## Dependencies

- OpenTK 4.9.4 (for audio playback)

## Links

- [GitHub Repository](https://github.com/Atoyr/Aivis-net)
- [Aivis Project](https://aivis-project.com)

## License

© 2025 Ryota Uchiyama
