# ユーザー API

`AivisUsersClient` は、アカウント情報を取得するクライアントです。
自分自身の情報（クレジット残高を含む）と、任意のユーザーの公開情報を取得できます。

```csharp
using Aivis;

var options = new AivisClientOptions(apiKey);
var usersClient = new AivisUsersClient(options);
```

## 自分の情報を取得する

`GetMe` は、API キーに紐づくアカウントの情報を返します。**API キーが必要です。**

```csharp
var me = await usersClient.GetMe();

Console.WriteLine($"ハンドル       : @{me.Handle}");
Console.WriteLine($"名前           : {me.Name}");
Console.WriteLine($"メールアドレス : {me.Email}");
Console.WriteLine($"クレジット残高 : {me.CreditBalance}");
```

### `UserResponseForMe` の主なメンバー

| メンバー | 型 | 内容 |
|---|---|---|
| `Handle` | `string` | ハンドル（識別子） |
| `Name` / `Description` / `IconUrl` | `string` | プロフィール情報 |
| `Email` | `string` | メールアドレス |
| `BirthYear` / `Gender` | `int?` / `Gender?` | 生年・性別（未設定なら `null`） |
| `AccountType` | `AccountType` | `User` / `Official` / `Admin` |
| `AccountStatus` | `AccountStatus` | `Active` / `Suspended` など |
| `SocialLinks` | `SocialLink[]` | ソーシャルリンク一覧 |
| `CreditBalance` | `int` | クレジット残高 |
| `IsLowBalanceNotificationEnabled` | `bool` | 残高不足の通知設定 |
| `LowBalanceThreshold` | `int` | 残高警告のしきい値 |
| `AivmModels` | `AivmModelsResponse[]` | 自身が公開しているモデル |
| `LikedAivmModels` | `AivmModelsResponse[]` | お気に入りしたモデル |
| `CreatedAt` / `UpdatedAt` | `DateTime` | 作成日時・更新日時 |

### クレジット残高を確認してから合成する

従量課金プランでは、合成前に残高を確認しておくと 402 エラーを避けられます。

```csharp
var me = await usersClient.GetMe();

if (me.CreditBalance <= me.LowBalanceThreshold)
{
    Console.Error.WriteLine($"クレジット残高が少なくなっています: {me.CreditBalance}");
}

var ttsClient = new AivisTTSClient(options);
using var audioStream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);
```

合成後の残高は `SynthesizeWithContentsAsync` の戻り値からも取得できます。
こちらはレスポンスヘッダー由来のため、追加のリクエストが不要です。

```csharp
var contents = await ttsClient.SynthesizeWithContentsAsync(modelUuid, text);
Console.WriteLine($"使用: {contents.CreditsUsed} / 残り: {contents.CreditsRemaining}");
```

## 他のユーザーの情報を取得する

`GetUserInfo` は、ハンドルを指定して公開情報を取得します。**API キーは不要です。**

```csharp
var options = new AivisClientOptions();   // API キーなしで生成できる
var usersClient = new AivisUsersClient(options);

var user = await usersClient.GetUserInfo("aivis_project");

Console.WriteLine($"@{user.Handle} / {user.Name}");
Console.WriteLine(user.Description);
```

戻り値は `UserResponseWithAivmModels` で、メールアドレスやクレジット残高などの
非公開情報は含まれません。

| メンバー | 型 | 内容 |
|---|---|---|
| `Handle` / `Name` / `Description` / `IconUrl` | `string` | プロフィール情報 |
| `AccountType` | `AccountType` | `User` / `Official` / `Admin` |
| `AccountStatus` | `AccountStatus` | `Active` / `Suspended` など |
| `SocialLinks` | `SocialLink[]` | ソーシャルリンク一覧 |
| `AivmModels` | `AivmModelsResponse[]` | 公開しているモデル |

## 例外

| 例外 | 発生条件 |
|---|---|
| `ArgumentException` | `GetMe` を API キー未設定で呼び出した |
| `UnauthorizedAccessException` | API キーが無効（401） |
| `NotSupportedException` | 指定したユーザーが存在しない（404、`GetUserInfo` のみ） |
| `HttpRequestException` | パラメータ不正（422）、その他の失敗 |
| `InvalidOperationException` | レスポンスが空だった |

```csharp
try
{
    var me = await usersClient.GetMe();
}
catch (UnauthorizedAccessException)
{
    Console.Error.WriteLine("API キーが無効です。");
}
```

> [!NOTE]
> `AivisUsersClient` は API キーなしでも生成できます。
> API キーの有無は `GetMe` の呼び出し時に検証され、未設定なら `ArgumentException` が発生します。

## サンプル

実行できるサンプルは
[sample/Users](https://github.com/Atoyr/Aivis.NET/tree/main/sample/Users) にあります。

```bash
dotnet run -c Release --project sample/Users/Aivis.Sample.Users.csproj
```
