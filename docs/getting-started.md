# Getting Started

## 対応バージョン
`.NET 8.0`
`.NET 9.0`

## パッケージの追加

``` 
dotnet add package Aivis.Net
```


## 使用方法

### 基本的なTTS

```C#
var options = new AivisClientOptions("your-api-key");
var client = new AivisTTSClient(options);

// 音声ストリームのみを取得
using var audioStream = await client.SynthesizeStreamAsync("model-uuid", "こんにちは、世界！");

// 音声とメタデータを取得
var contents = await client.SynthesizeWithContentsAsync("model-uuid", "ここにテキストを入力");
Console.WriteLine($"文字数: {contents.CharacterCount}");
Console.WriteLine($"使用クレジット: {contents.CreditsUsed}");
```

### リアルタイムストリーミング再生
> [!NOTE]
> FFmpegとOpenALがインストールされている必要があります。
> インストール方法は[音声再生の利用](#音声再生の利用)を参照してください。
```csharp
using Aivis;
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


### NAudioを使った音声再生

> [!NOTE]
> NAudioのSpeakerのサンプルは[sample/NAudio/NAudioSpeaker](https://github.com/Atoyr/Aivis.NET/tree/main/sample/NAudio)にあります。
``` C#
Aivis.AivisClientOptions options = new(apiKey!);
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);

Aivis.Speakers.ISpeaker speaker = new Aivis.Sample.NAudio.NAudioSpeaker();
await speaker.PlayAsync(stream);
```

# 音声再生の利用
同封している`MP3Speaker`は`ffmpeg`と`OpenAL`を使用しています。
そのため、`ffmpeg`と`OpenAL`のインストールが必要となります。
また、Windowsに限り`NAudio`を使用することで各種ツールのインストールが不要です。

## ffmpegとOpenALのインストール
> [!NOTE]
> どのOSでも最後に`ffmpeg -version`で確認してください。

### windows
コンソールから`winget`コマンドでインストールできます。
```
winget install -e --id Gyan.FFmpeg

winget install --id=CreativeTechnology.OpenAL -e
```

手動インストールを行う場合は、[ffmpegのダウンロードページ](https://ffmpeg.org/download.html#build-windows)からダウンロード・インストールを行い、環境変数PATHに追加してください。

### macOS
Homebrewを使ってインストールできます。
```
brew install ffmpeg

brew install openal-soft
```

### Linux

**Ubuntu / Debian 系**
```
sudo apt update

sudo apt install ffmpeg
sudo apt install -y libopenal1
```

**Arch Linux**
```

sudo pacman -S ffmpeg
sudo pacman -S openal
```

## NAudioを使った音声再生
NAudioのSpeakerのサンプルは[sample/NAudio/NAudioSpeaker](https://github.com/Atoyr/Aivis.NET/tree/main/sample/NAudio)にあります。

## 依存関係

- OpenTK 4.9.4（音声再生用）

## リンク

- [GitHubリポジトリ](https://github.com/Atoyr/Aivis.NET)
- [Aivis Project](https://aivis-project.com)
