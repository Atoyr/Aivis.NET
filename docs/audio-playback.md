# 音声再生

Aivis.NET は音声再生のインターフェイス `ISpeaker` と、その既定実装である `MP3Speaker` を提供します。

`MP3Speaker` はデコードに **FFmpeg**、再生に **OpenAL** を使うため、
実行環境にこの 2 つがインストールされている必要があります。
Windows のみを対象とする場合は、NAudio を使った実装で代替できます。

## FFmpeg と OpenAL のインストール

> [!NOTE]
> インストール後、`ffmpeg -version` が実行できることを確認してください。

### Windows

`winget` でインストールできます。

```powershell
winget install -e --id Gyan.FFmpeg
winget install -e --id CreativeTechnology.OpenAL
```

手動でインストールする場合は、[FFmpeg のダウンロードページ](https://ffmpeg.org/download.html#build-windows)から
取得し、実行ファイルのあるディレクトリを環境変数 `PATH` に追加してください。

### macOS

Homebrew でインストールできます。

```bash
brew install ffmpeg
brew install openal-soft
```

### Linux

**Ubuntu / Debian 系**

```bash
sudo apt update
sudo apt install -y ffmpeg libopenal1
```

**Arch Linux**

```bash
sudo pacman -S ffmpeg openal
```

## MP3Speaker を使う

```csharp
using Aivis;
using Aivis.Speakers;

var options = new AivisClientOptions(apiKey);
var client = new AivisTTSClient(options);

using var audioStream = await client.SynthesizeStreamAsync(modelUuid, "こんにちは、世界！");

using MP3Speaker speaker = new();
await speaker.PlayAsync(audioStream);
```

`PlayAsync` は再生が完了するまで待機します。

> [!NOTE]
> 1 つの `MP3Speaker` インスタンスで同時に複数の音声は再生できません。
> 再生中に `PlayAsync` を呼び出すと `InvalidOperationException` が発生します。
> 連続して読み上げる場合は前の `PlayAsync` の完了を待つか、インスタンスを分けてください。

> [!IMPORTANT]
> `MP3Speaker` は `IDisposable` です。FFmpeg のプロセスと OpenAL のリソースを解放するため、
> `using` で破棄するか、明示的に `Dispose()` を呼び出してください。

> [!WARNING]
> `MP3Speaker` は名前のとおり **MP3 を前提**にデコードします。
> `TalkToSpeechOptions.SetOutputFormat` で `MP3` 以外を指定した音声は、
> 既定の設定のままでは再生できません。形式を変える場合は
> [FFmpeg の設定を変更する](#ffmpeg-の設定を変更する)を参照してください。

### バッファの調整

コンストラクタでバッファ数と 1 バッファあたりの長さ（ミリ秒）を指定できます。
既定は 4 バッファ × 100ms です。

```csharp
// 低遅延寄り: バッファを小さくする
using MP3Speaker speaker = new(bufferCount: 4, bufferMillis: 50);

// 安定寄り: バッファを大きくする（音切れしやすい環境向け）
using MP3Speaker stableSpeaker = new(bufferCount: 8, bufferMillis: 200);
```

バッファを小さくすると再生開始は早くなりますが、
処理が追いつかない環境では音切れが発生しやすくなります。

### 音量の制御

`Volume` プロパティで再生音量を 0.0〜1.0 の範囲で変更できます。

```csharp
using MP3Speaker speaker = new();
speaker.Volume = 0.5f;
await speaker.PlayAsync(audioStream);
```

> [!TIP]
> `MP3Speaker.Volume` は再生側の音量です。
> 合成される音声そのものの音量を変えたい場合は
> [`TalkToSpeechOptions.Volume`](tts-options.md#声の調整) を使います。

### 再生の停止

再生中に停止したい場合は `Stop()` を呼び出すか、`CancellationToken` を渡します。

```csharp
using var cts = new CancellationTokenSource();
using MP3Speaker speaker = new();

var playTask = speaker.PlayAsync(audioStream, cts.Token);

// 別の処理から停止する
cts.Cancel();

await playTask;
```

### FFmpeg の設定を変更する

`ConfigureBuildOptions` で `FFmpegDecodeOptions` を変更できます。

```csharp
using MP3Speaker speaker = new();

speaker.ConfigureBuildOptions(opt =>
{
    opt.SetFFmpegPath("/usr/local/bin/ffmpeg")   // PATH に無い場合
       .SetLogLevel("warning")
       .SetSampleRate(48000)
       .SetChannels(2);
});

await speaker.PlayAsync(audioStream);
```

| 設定 | 既定値 | 内容 |
|---|---|---|
| `FFmpegPath` | `ffmpeg` | FFmpeg の実行パス |
| `LogLevel` | `error` | `quiet` / `error` / `warning` / `info` |
| `SampleRate` | `48000` | デコード後のサンプリングレート |
| `Channels` | `2` | デコード後のチャンネル数（1 または 2） |
| `InputFormat` | `mp3` | 入力形式。`init` 専用のため初期化時のみ指定できます |
| `ExtraInputArgs` / `ExtraOutputArgs` | 空 | FFmpeg に渡す追加引数 |

> [!CAUTION]
> `SetFFmpegPath` や追加引数に、外部入力由来の値をそのまま渡さないでください。
> 意図しないコマンドが実行される恐れがあります。詳細は
> [SECURITY.md](https://github.com/Atoyr/Aivis.NET/blob/main/SECURITY.md) を参照してください。

## 再生デバイスを調べる

`Speakers.Device` で OpenAL が認識している再生デバイスを列挙できます。
音が出ない場合の切り分けに使えます。

```csharp
using Aivis.Speakers;

foreach (var name in Device.ListPlaybackDevices())
{
    Console.WriteLine(name);
}

Console.WriteLine($"既定のデバイス: {Device.DefaultPlaybackDevice()}");
```

## NAudio で再生する（Windows）

Windows のみを対象とする場合、NAudio を使えば FFmpeg も OpenAL も不要です。
ライブラリ本体には含まれていないため、`ISpeaker` を自前で実装します。

実装例は
[sample/NAudio/NAudioSpeaker.cs](https://github.com/Atoyr/Aivis.NET/blob/main/sample/NAudio/NAudioSpeaker.cs)
にあります。

```csharp
using var audioStream = await client.SynthesizeStreamAsync(modelUuid, text);

ISpeaker speaker = new Aivis.Sample.NAudio.NAudioSpeaker();
await speaker.PlayAsync(audioStream);
```

## 独自の再生実装を作る

`ISpeaker` は次のメソッドだけを持つ最小のインターフェイスです。
これを実装すれば、任意の再生方式を差し込めます。

```csharp
public interface ISpeaker
{
    Task PlayAsync(Stream audioStream, CancellationToken cancellationToken = default);
}
```

たとえば、再生せずにファイルへ保存する実装は次のように書けます。

```csharp
public sealed class FileSpeaker : ISpeaker
{
    private readonly string _path;

    public FileSpeaker(string path) => _path = path;

    public async Task PlayAsync(Stream audioStream, CancellationToken cancellationToken = default)
    {
        using var file = File.Create(_path);
        await audioStream.CopyToAsync(file, cancellationToken);
    }
}
```

## うまく再生されないとき

音が出ない、途切れる、例外が出るといった場合は
[トラブルシューティング](troubleshooting.md#音声再生のトラブル)を参照してください。
