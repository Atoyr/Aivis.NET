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
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "こんにちは、世界！");

// 音声を再生
var speaker = new Speaker();
await speaker.PlayAsync(MediaType.MP3, contents.Audio);
```

## 機能

- ✅ Aivis APIを使用したテキスト音声合成
- ✅ NAudioによる音声再生サポート
- ✅ レスポンスメタデータ（課金情報、文字数、レート制限など）
- ✅ .NET 8.0および.NET 9.0サポート
- ✅ async/awaitパターン

## 使用方法

### 基本的なTTS

```csharp
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// 音声ストリームのみを取得
var audioStream = await client.SynthesizeAsync("model-uuid", "ここにテキストを入力");

// 音声とメタデータを取得
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "ここにテキストを入力");
Console.WriteLine($"文字数: {contents.CharacterCount}");
Console.WriteLine($"使用クレジット: {contents.CreditsUsed}");
```

### 音声再生

```csharp
var speaker = new Speaker();
await speaker.PlayAsync(MediaType.MP3, audioData);
```

## 依存関係

- NAudio 2.2.1（音声再生用）

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
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "Hello, world!");

// Play audio
var speaker = new Speaker();
await speaker.PlayAsync(MediaType.MP3, contents.Audio);
```

## Features

- ✅ Text-to-Speech synthesis via Aivis API
- ✅ Audio playback support with NAudio
- ✅ Response metadata (billing, character count, rate limits)
- ✅ Support for .NET 8.0 and .NET 9.0
- ✅ Async/await pattern

## Usage

### Basic TTS

```csharp
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// Get audio stream only
var audioStream = await client.SynthesizeAsync("model-uuid", "Your text here");

// Get audio with metadata
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "Your text here");
Console.WriteLine($"Character Count: {contents.CharacterCount}");
Console.WriteLine($"Credits Used: {contents.CreditsUsed}");
```

### Audio Playback

```csharp
var speaker = new Speaker();
await speaker.PlayAsync(MediaType.MP3, audioData);
```

## Dependencies

- NAudio 2.2.1 (for audio playback)

## Links

- [GitHub Repository](https://github.com/Atoyr/Aivis-net)
- [Aivis Project](https://aivis-project.com)

## License

© 2025 Ryota Uchiyama