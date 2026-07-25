<div align="center">

  ![Aivis.NET Logo](/docs/logo_dark.svg#gh-light-mode-only)
  ![Aivis.NET Logo](/docs/logo_light.svg#gh-dark-mode-only)

  <br/>

  <a href="https://www.nuget.org/packages/Aivis.Net">
    <img alt="NuGet Version" src="https://img.shields.io/nuget/v/Aivis.Net">
  </a>

  <a href="https://github.com/Atoyr/Aivis.NET/releases">
    <img alt="GitHub Release" src="https://img.shields.io/github/v/release/Atoyr/Aivis.NET">
  </a>

  <a href="https://github.com/Atoyr/Aivis.NET/actions/workflows/ci.yml">
    <img alt="GitHub Actions CI Workflow Status" src="https://img.shields.io/github/actions/workflow/status/Atoyr/Aivis.NET/ci.yml">
  </a>

  #

</div>
Aivis.NET は [Aivis Cloud API](https://aivis-project.com) の非公式 C# ライブラリです。
テキスト読み上げ（Text-to-Speech）、音声合成モデルの検索、ユーザー情報の取得に対応しています。

📖 **[ドキュメントサイト](https://atoyr.github.io/Aivis.NET/)** — 使い方ガイドと API リファレンス

# Getting Start.

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
> [!IMPORTANT]
> FFmpegとOpenALがインストールされている必要があります。
> インストール方法は[音声再生の利用](#音声再生の利用)を参照してください。
``` C#
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);

using Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(stream);
```

音声データとあわせて課金情報を取得する
``` C#
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
var contents = await ttsClient.SynthesizeWithContentsAsync(modelUuid, text);

Console.WriteLine($"文字数: {contents.CharacterCount}");
Console.WriteLine($"使用クレジット: {contents.CreditsUsed}");

using Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(contents.AudioStream);
```

読み上げ方をカスタマイズする
``` C#
var ttsOptions = new Aivis.TalkToSpeechOptions()
    .SetStyle("ノーマル")
    .SetSpeakingRate(1.2)
    .SetVolume(0.8);

using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text, ttsOptions);
```
> 指定できるオプションの一覧は[TTS オプション](https://atoyr.github.io/Aivis.NET/docs/tts-options.html)を参照してください。

NAudioを使った音声再生
> NAudioのSpeakerのサンプルは[sample/NAudio/NAudioSpeaker](./sample/NAudio/NAudioSpeaker.cs)にあります。
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

## ffmpegのインストール
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
NAudioのSpeakerのサンプルアプリは[sample/NAudio](./sample/NAudio)にあります。

# サンプルアプリ

| サンプル | 内容 |
|---|---|
| [StreamPlay](./sample/StreamPlay) | ストリーミング再生 |
| [Contents](./sample/Contents) | 音声データと課金情報の取得 |
| [SearchModels](./sample/SearchModels) | 音声合成モデルの検索 |
| [Users](./sample/Users) | ユーザー情報の取得 |
| [NAudio](./sample/NAudio) | NAudio による再生（Windows） |

```bash
dotnet run -c Release --project sample/StreamPlay/Aivis.Sample.StreamPlay.csproj
```

# ドキュメント

- 📘 [Getting Started](https://atoyr.github.io/Aivis.NET/docs/getting-started.html) — 導入と基本的な使い方
- 🎚️ [TTS オプション](https://atoyr.github.io/Aivis.NET/docs/tts-options.html) — 読み上げパラメータの一覧
- 🔍 [モデル検索](https://atoyr.github.io/Aivis.NET/docs/model-search.html) — 音声合成モデルを探す
- 👤 [ユーザー API](https://atoyr.github.io/Aivis.NET/docs/users.html) — アカウント情報の取得
- 🔊 [音声再生](https://atoyr.github.io/Aivis.NET/docs/audio-playback.html) — `ISpeaker` と再生環境の準備
- 🛠️ [トラブルシューティング](https://atoyr.github.io/Aivis.NET/docs/troubleshooting.html) — よくあるエラーと対処
- 🏗️ [アーキテクチャ](https://atoyr.github.io/Aivis.NET/docs/architecture.html) — 内部構造と拡張ポイント
- 📚 [API リファレンス](https://atoyr.github.io/Aivis.NET/api/Aivis.html)

## ❓ 質問・不具合の報告

- バグ報告や機能要望は [Issueページ](https://github.com/Atoyr/Aivis.NET/issues) よりお願いします
- ライブラリの使い方の質問も歓迎しています！`[Question]` ラベルを付けて投稿してください
- セキュリティに関わる問題は [SECURITY.md](./SECURITY.md) の手順に従って非公開でご報告ください

# コントリビューション

Pull Request を歓迎しています。開発環境のセットアップやコーディング規約は
[CONTRIBUTING.md](./CONTRIBUTING.md) を参照してください。
参加にあたっては [行動規範](./CODE_OF_CONDUCT.md) に従うものとします。

# ライセンス
MIT

依存する第三者ソフトウェアのライセンスは [THIRD-PARTY-LICENSES.md](./THIRD-PARTY-LICENSES.md) を参照してください。
