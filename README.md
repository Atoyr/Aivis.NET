<div align="center">

  ![Aivis.NET Logo](/docs/logo_dark.svg#gh-light-mode-only)
  ![Aivis.NET Logo](/docs/logo_light.svg#gh-dark-mode-only)

  <br/>

  <a href="https://www.nuget.org/packages/Aivis.Net">
    <img alt="NuGet Version" src="https://img.shields.io/nuget/v/Aivis.Net">
  </a>

  <a href="https://github.com/Atoyr/Aivis-net/releases">
    <img alt="GitHub Release" src="https://img.shields.io/github/v/release/Atoyr/Aivis-net">
  </a>

  <a href="https://github.com/Atoyr/Aivis-net/actions/workflows/ci.yml">
    <img alt="GitHub Actions CI Workflow Status" src="https://img.shields.io/github/actions/workflow/status/Atoyr/Aivis-net/ci.yml">
  </a>

  #

</div>
Aivis.NET は Aivis APIの非公式ライブラリです。(https://aivis-project.com)

# 音声再生の利用
同封している`MP3Speaker`は`ffmpeg`と`OpenAL`を使用しています。
そのため、`ffmpeg`と`OpenAL`のインストールが必要となります。
また、Windowsに限り`NAudio`を使用することで各種ツールのインストールが不要です。

## ffmpegのインストール
> どのOSでも最後に`ffmepg -version`で確認してください。

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


# Getting Start.

## 対応バージョン
`.NET 8.0`
`.NET 9.0`

## パッケージの追加


``` 
dotnet add package Aivis.Net
```



## 使用例

Text-to-Speech
``` C#
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
var stream = await ttsClient.SynthesizeAsync(modelUuid, text);

Aivis.Speaker speaker = new();
await speaker.PlayAsync(Aivis.MediaType.MP3, stream);
```

ストリーミング再生
``` C#
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);

Aivis.Speaker speaker = new();
await speaker.PlayAsync(Aivis.MediaType.MP3, stream);
```

## ❓ 質問・不具合の報告

- バグ報告や機能要望は [Issueページ](https://github.com/Atoyr/Aivis-net/issues) よりお願いします
- ライブラリの使い方の質問も歓迎しています！`[Question]` ラベルを付けて投稿してください

# ライセンス
MIT
