# モデル検索

`AivisModelClient` は、公開されている音声合成モデル（AIVM モデル）の検索・詳細取得・
ダウンロード URL の取得を行うクライアントです。

これらのエンドポイントは公開情報を返すため、**API キーがなくても利用できます**。

```csharp
using Aivis;

var options = new AivisClientOptions();   // API キーなしで生成できる
var modelClient = new AivisModelClient(options);
```

API キーを設定しても構いません。その場合でも、モデル検索の各メソッドは認証ヘッダーを送信しません。

## モデルを検索する

`SearchVoiceModelsOptions` で条件を指定し、`SearchModels` を呼び出します。

```csharp
var searchOptions = new SearchVoiceModelsOptions()
    .SetKeyword("アナウンサー")
    .SetLimit(10);

var result = await modelClient.SearchModels(searchOptions);

Console.WriteLine($"該当件数: {result.Total}");

foreach (var model in result.AivmModels)
{
    Console.WriteLine($"{model.Name} ({model.AivmModelUuid})");
    Console.WriteLine($"  {model.Description}");
    Console.WriteLine($"  タグ: {string.Join(", ", model.Tags.Select(t => t.Name))}");
}
```

### 検索条件

| プロパティ | 型 | 既定値 | 説明 |
|---|---|---|---|
| `Keyword` | `string?` | `null` | 検索キーワード |
| `Tags` | `string[]?` | `null` | タグ |
| `Categories` | `string[]?` | `null` | カテゴリー |
| `VoiceTimbres` | `string[]?` | `null` | 声質 |
| `LicenceTypes` | `string[]?` | `null` | ライセンス種別 |
| `Sort` | `string` | `download` | 並び順 |
| `Page` | `int` | `1` | ページ番号（1 以上） |
| `Limit` | `int` | `24` | 1 ページあたりの件数（1〜30） |

`SetXxx()` メソッドは自身を返すため、続けて記述できます。
配列を取る条件は可変長引数で渡せます。

```csharp
var searchOptions = new SearchVoiceModelsOptions()
    .SetKeyword("ナレーション")
    .SetTags("女性", "落ち着いた")
    .SetCategories("OriginalCharacter")
    .SetVoiceTimbres("AdultFemale")
    .SetLicenceTypes("ACML 1.0")
    .SetSort("download")
    .SetPage(1)
    .SetLimit(30);
```

> [!NOTE]
> `Page` は 1 未満、`Limit` は 1〜30 の範囲外を指定すると `ArgumentOutOfRangeException` が発生します。

### 指定できる値

**カテゴリー（`Categories`）**

`ExistingCharacter` / `OriginalCharacter` / `LivingPerson` / `DeceasedPerson` / `FictionalPerson` / `Other`

**声質（`VoiceTimbres`）**

`YoungMale` / `YoungFemale` / `YouthfulMale` / `YouthfulFemale` / `AdultMale` / `AdultFemale` /
`MiddleAgedMale` / `MiddleAgedFemale` / `ElderlyMale` / `ElderlyFemale` / `Neutral` / `Baby` /
`Mechanical` / `Other`

**ライセンス種別（`LicenceTypes`）**

`ACML 1.0` / `ACML-NC 1.0` / `CC0` / `Custom` / `Internal`

> [!WARNING]
> API には提供元を絞り込む `tiers`（`Official` / `Community`）パラメータがありますが、
> `SearchVoiceModelsOptions` では**未対応**です。

### ページング

`AivmModelsResponse.Total` は条件に一致する総件数、`AivmModels` は現在のページの結果です。
両者の件数は一致しません。

```csharp
var searchOptions = new SearchVoiceModelsOptions()
    .SetKeyword("ナレーション")
    .SetLimit(30);

var first = await modelClient.SearchModels(searchOptions);
var pageCount = (int)Math.Ceiling(first.Total / 30.0);

for (var page = 2; page <= pageCount; page++)
{
    var next = await modelClient.SearchModels(searchOptions.SetPage(page));
    // ...
}
```

## モデルの詳細を取得する

モデル UUID を指定して詳細情報を取得します。
検索結果と同じ `AivmModelResponse` が返りますが、単一モデルの取得に使います。

```csharp
var model = await modelClient.GetModelDetail("a59cb814-0083-4369-8542-f51a29e72af7");

Console.WriteLine($"{model.Name} / {model.Category} / {model.VoiceTimbre}");
Console.WriteLine($"お気に入り数: {model.LikeCount}");
Console.WriteLine($"作成者: {model.User.Name} (@{model.User.Handle})");
```

### 話者とスタイルを調べる

[TTS オプション](tts-options.md)で指定する `SpeakerUuid` / `StyleId` / `StyleName` は、
モデル詳細から取得できます。

```csharp
foreach (var speaker in model.Speakers)
{
    Console.WriteLine($"話者: {speaker.Name} ({speaker.AivmSpeakerUuid})");

    foreach (var style in speaker.Styles)
    {
        Console.WriteLine($"  スタイル: {style.Name} (LocalId: {style.LocalId})");
    }
}
```

- `Speaker.AivmSpeakerUuid` → `TalkToSpeechOptions.SpeakerUuid`
- `Style.LocalId` → `TalkToSpeechOptions.StyleId`
- `Style.Name` → `TalkToSpeechOptions.StyleName`

```csharp
var speaker = model.Speakers.First();
var style = speaker.Styles.First(s => s.Name == "上機嫌");

var ttsOptions = new TalkToSpeechOptions()
    .SetSpeakerUuid(speaker.AivmSpeakerUuid)
    .SetStyle(style.LocalId);
```

### `AivmModelResponse` の主なメンバー

| メンバー | 型 | 内容 |
|---|---|---|
| `AivmModelUuid` | `Guid` | モデル UUID。TTS で使用します |
| `Name` / `Description` / `DetailedDescription` | `string` | 名前・説明・詳細説明 |
| `User` | `UserResponse` | 作成者の公開情報 |
| `Category` | `Category` | カテゴリー |
| `VoiceTimbre` | `VoiceTimbre` | 声質 |
| `Visibility` | `Visibility` | 公開範囲（`Public` / `Private` / `AdminHidden`） |
| `Speakers` | `Speaker[]` | 話者一覧（スタイルを含む） |
| `ModelFiles` | `ModelFile[]` | モデルファイル一覧 |
| `Tags` | `Tag[]` | タグ一覧 |
| `LikeCount` / `IsLiked` | `int` / `bool` | お気に入り数と自身の状態 |
| `CreatedAt` / `UpdatedAt` | `DateTime` | 作成日時・更新日時 |

> [!NOTE]
> `IsLiked` は認証済みの場合のみ意味を持ちます。
> `AivisModelClient` は認証ヘッダーを送らないため、常に `false` になります。

## ダウンロード URL を取得する

モデルファイルの実体を取得するための署名付き URL を発行します。

```csharp
var url = await modelClient.GetDownloadUrl("a59cb814-0083-4369-8542-f51a29e72af7");
Console.WriteLine(url);

// AIVMX 形式を指定する
var aivmxUrl = await modelClient.GetDownloadUrl(modelUuid, ModelType.AIVMX);
```

| `ModelType` | 内容 |
|---|---|
| `AIVM`（既定） | AIVM 形式（Safetensors ベース） |
| `AIVMX` | AIVMX 形式（ONNX ベース） |

このメソッドはリダイレクトを追わず、`Location` ヘッダーの URL を文字列で返します。
実際のダウンロードは `HttpClient` などで別途行ってください。

```csharp
using var http = new HttpClient();
using var response = await http.GetAsync(url);
using var file = File.Create("model.aivmx");
await response.Content.CopyToAsync(file);
```

> [!NOTE]
> ダウンロードした AIVM / AIVMX ファイルはローカル推論エンジン
> （[AivisSpeech](https://aivis-project.com/speech/) など）で利用するものです。
> Aivis Cloud API の音声合成では、モデル UUID を指定するだけでよく、ファイルの取得は不要です。

## 例外

| 例外 | 発生条件 |
|---|---|
| `ArgumentException` | モデル ID が `null` または空文字 |
| `KeyNotFoundException` | 指定したモデルが存在しない（404、`GetModelDetail` のみ） |
| `HttpRequestException` | パラメータ不正（422）、`Location` ヘッダー欠落、その他の失敗 |

```csharp
try
{
    var model = await modelClient.GetModelDetail(modelUuid);
}
catch (KeyNotFoundException)
{
    Console.Error.WriteLine("モデルが見つかりませんでした。");
}
catch (HttpRequestException ex)
{
    Console.Error.WriteLine($"取得に失敗しました: {ex.Message}");
}
```

## サンプル

実行できるサンプルは
[sample/SearchModels](https://github.com/Atoyr/Aivis.NET/tree/main/sample/SearchModels) にあります。

```bash
dotnet run -c Release --project sample/SearchModels/Aivis.Sample.Models.csproj
```
