# コントリビューションガイド

Aivis.NET への貢献に興味を持っていただきありがとうございます。
このドキュメントは、バグ報告から Pull Request のマージまでの流れをまとめたものです。

このプロジェクトに参加するすべての方は [行動規範](./CODE_OF_CONDUCT.md) に従うものとします。

## 目次

- [貢献の方法](#貢献の方法)
- [開発環境のセットアップ](#開発環境のセットアップ)
- [ビルド・テスト](#ビルドテスト)
- [コーディング規約](#コーディング規約)
- [ドキュメントの更新](#ドキュメントの更新)
- [コミットメッセージ](#コミットメッセージ)
- [Pull Request](#pull-request)
- [リリース手順](#リリース手順)

## 貢献の方法

### バグ報告・機能要望・質問

すべて [Issue](https://github.com/Atoyr/Aivis.NET/issues) で受け付けています。
用途に応じたテンプレートを用意しているので、該当するものを選んでください。

| 種類 | テンプレート |
|---|---|
| バグ報告 | Bug report |
| 機能要望 | Feature request |
| 使い方の質問 | Question |
| リファクタリング提案 | Refactor |
| その他の雑務 | Chore |

セキュリティに関わる問題は Issue ではなく [SECURITY.md](./SECURITY.md) の手順に従って報告してください。

### コードの貢献

何に取り組むか探している場合は、[STATE.md](./STATE.md) に未実装機能・既知の不具合・
今後の課題を優先度つきでまとめています。

1. Issue を立てて、対応方針について合意を取ってください（軽微な修正は不要です）
2. リポジトリを fork し、作業ブランチを作成します
3. 変更を加え、`dotnet build` / `dotnet test` / `dotnet format` が通ることを確認します
4. Pull Request を作成します

## 開発環境のセットアップ

### 必要なもの

- .NET SDK 6.0 / 7.0 / 8.0 / 9.0 / 10.0
  - ライブラリはこの 5 つのターゲットフレームワークすべてに対してビルドされるため、すべての SDK が必要です
  - [mise](https://mise.jdx.dev/) を使っている場合は、リポジトリルートで `mise install` を実行すると `mise.toml` の定義に従って一括導入できます
- FFmpeg / OpenAL（`MP3Speaker` を使うテスト・サンプルを実行する場合のみ）
  - インストール方法は [docs/audio-playback.md](./docs/audio-playback.md) を参照してください

### クローン

```bash
git clone https://github.com/Atoyr/Aivis.NET.git
cd Aivis.NET
dotnet restore src/Aivis.Net.csproj
```

## ビルド・テスト

CI（`.github/workflows/ci.yml`）で実行しているコマンドと同じものをローカルでも実行できます。

```bash
# ライブラリのビルド
dotnet restore src/Aivis.Net.csproj
dotnet build   src/Aivis.Net.csproj -c Release --no-restore

# テスト
dotnet restore tests/Aivis.Tests.csproj
dotnet build   tests/Aivis.Tests.csproj -c Release --no-restore
dotnet test    tests/Aivis.Tests.csproj -c Release --no-build

# カバレッジ付きテスト
dotnet test tests/Aivis.Tests.csproj --collect:"XPlat Code Coverage"

# フォーマット検証（CI と同じ条件）
dotnet format src/Aivis.Net.csproj   --verify-no-changes
dotnet format tests/Aivis.Tests.csproj --verify-no-changes
```

> [!IMPORTANT]
> CI は `dotnet format --verify-no-changes` で書式を検証します。
> 差分があると失敗するため、PR を出す前に `dotnet format` を実行してください。

### サンプルアプリの実行

```bash
dotnet run -c Release --project sample/StreamPlay/Aivis.Sample.StreamPlay.csproj
dotnet run -c Release --project sample/SearchModels/Aivis.Sample.Models.csproj
dotnet run -c Release --project sample/Users/Aivis.Sample.Users.csproj
dotnet run -c Release --project sample/Contents/Aivis.Sample.Contents.csproj
dotnet run -c Release --project sample/NAudio/Aivis.Sample.NAudio.csproj   # Windows のみ
```

実行時に API キーとモデル UUID をコンソールから入力します。API キーは
[Aivis Project のダッシュボード](https://hub.aivis-project.com/cloud-api/api-keys)で発行してください。

## コーディング規約

書式は `.editorconfig` で定義されており、`dotnet format` で自動適用できます。
主なルールは次のとおりです。

| 項目 | ルール |
|---|---|
| インデント | 半角スペース 4 |
| 改行コード | CRLF |
| `using` | グループ化＋並び替え（`System` を先頭） |
| 型・メンバー | PascalCase |
| インターフェイス | `I` 接頭辞（例: `ISpeaker`） |
| 非公開フィールド | `_camelCase` |
| 非公開 static フィールド | `s_camelCase` |
| ローカル変数・引数 | `camelCase` |
| ブレース | 単文でも省略しない |
| `var` | 右辺から型が明白な場合のみ使用 |

### 設計上の約束

- API スキーマの DTO は `src/Schemas/` に置き、OpenAPI のコンポーネントスキーマ名と揃えます（例: `AivmModelResponse`）
- JSON のシリアライズは `System.Text.Json` を使い、文字列 enum には `EnumMemberStringEnumConverter` を使います
- API 領域ごとにクライアントクラス（`Aivis{領域}Client`）を作り、`AivisClientOptions` を受け取って `IHttpClientProvider` 経由で通信します
- 公開メンバーには XML ドキュメントコメント（`/// <summary>`）を付けます。DocFX の API リファレンスに反映されます

詳しい構造は [docs/architecture.md](./docs/architecture.md) を参照してください。

## テストの書き方

- フレームワーク: xUnit / モック: Moq / カバレッジ: coverlet.collector
- `tests/` 配下に、`src/` の構成と対応する形で `*Tests.cs` を配置します
- HTTP 通信を伴うテストでは `tests/MockHttpClientProvider.cs` を使い、実際の API を呼ばないようにします
- `MP3Speaker` 関連のテストは FFmpeg と OpenAL を必要とします。CI では自動インストールされますが、ローカルでは事前に導入してください

## ドキュメントの更新

ドキュメントは用途ごとに置き場所が分かれています。変更内容に応じて対象を更新してください。

| ファイル | 用途 |
|---|---|
| `README.md` | GitHub のトップページ。導入と最小の使用例 |
| `NUGET_README.md` | NuGet パッケージの説明（日英併記）|
| `docs/*.md` | DocFX で公開するユーザーガイド。新規ページは `docs/toc.yml` にも追加 |
| `CHANGELOG.md` | リリースごとの変更履歴 |
| `STATE.md` | 未実装機能・既知の不具合・今後の課題。項目に着手したらチェックを更新 |
| `Aivis.Net.targets` | NuGet の `PackageReleaseNotes`。CHANGELOG と同じ内容を記載 |
| `CLAUDE.md` / `AGENTS.md` | AI コーディングエージェント向けの前提情報 |

### DocFX サイトのプレビュー

```bash
dotnet tool update -g docfx   # 未導入なら install
docfx ./docfx.json            # _site/ に出力
docfx serve _site             # ローカルで閲覧
```

生成物（`_site/`・`api/`）はコミット対象です。GitHub Pages への反映は
`.github/workflows/docs.yml` で自動化されています。

## コミットメッセージ

先頭に種別を表す接頭辞を付け、短い命令形で書きます。

```
feat: モデル検索にソート順の指定を追加
fix: OutputBitrate が wav 指定時に無視される問題を修正
docs: CONTRIBUTING.md を追加
```

使用している接頭辞: `feat:` / `fix:` / `docs:` / `chore:` / `refactor:` / `test:` / `update:` / `upgrade:` / `release:`

1 コミットの変更範囲はできるだけ絞ってください。

## Pull Request

- ベースブランチは `main` です
- `.github/pull_request_template.md` の項目を埋めてください
- 関連する Issue があれば `Fix #123` のように参照してください
- CI（フォーマット検証・ビルド・テスト・サンプルのビルド）がすべて成功している必要があります
- 破壊的変更を含む場合は、その旨と移行方法を PR の説明に明記してください

## リリース手順

メンテナー向けの手順です。

1. `CHANGELOG.md` に新バージョンの項目を追加する
2. `Aivis.Net.targets` の `PackageReleaseNotes` に同じ内容を追記する
3. `v1.2.3` 形式のタグで GitHub Release を作成する
4. `.github/workflows/publish.yml` が起動し、タグからバージョンを決定して NuGet.org と GitHub Packages へ発行する

バージョン番号は csproj に固定せず、タグから `-p:Version=` で注入されます。

## ライセンス

貢献されたコードは、このリポジトリと同じ [MIT ライセンス](./LICENSE) の下で公開されます。
