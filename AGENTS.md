# AGENTS.md

このリポジトリで作業する AI コーディングエージェント向けの手引きです。
人間の貢献者向けの詳細は [CONTRIBUTING.md](./CONTRIBUTING.md) を参照してください。

## ドキュメントの役割分担

作業を始める前に、目的に応じて次のドキュメントを参照してください。

| ドキュメント | 内容 |
|---|---|
| `AGENTS.md`（このファイル） | 作業手順・コマンド・規約・注意点。エージェントの入口 |
| `CLAUDE.md` | プロジェクト概要と Aivis Cloud API の仕様、エンドポイントの実装状況 |
| `STATE.md` | 未実装機能、既知の不具合、今後の課題と優先度 |
| `docs/architecture.md` | 内部構造、処理の流れ、拡張ポイント、実装と API 仕様の差異 |
| `CONTRIBUTING.md` | 人間向けの貢献手順、リリース手順 |
| `docs/*.md` | 利用者向けガイド（DocFX で公開） |

## プロジェクト構成

- `src/` — ライブラリ本体（`Aivis.Net.csproj`）
  - `src/Schemas/` — API の DTO・enum。OpenAPI のコンポーネントスキーマ名に合わせる
  - `src/Speakers/` — 音声再生（FFmpeg + OpenAL）
- `tests/` — xUnit テスト（`Aivis.Tests.csproj`）。`src/` の構成に対応させ `*Tests.cs` で命名
- `sample/` — 実行可能なサンプル（`Contents` / `NAudio` / `SearchModels` / `StreamPlay` / `Users`）
- `docs/` — DocFX のユーザーガイドとロゴ資産
- `api/` `_site/` — DocFX の生成物（コミット対象）
- `.github/` — CI ワークフロー、Issue / PR テンプレート

## コマンド

```bash
# 復元・ビルド
dotnet restore src/Aivis.Net.csproj
dotnet build   src/Aivis.Net.csproj -c Release --no-restore

# テスト
dotnet test tests/Aivis.Tests.csproj -c Release
dotnet test tests/Aivis.Tests.csproj --collect:"XPlat Code Coverage"   # カバレッジ

# フォーマット（CI と同じ検証）
dotnet format src/Aivis.Net.csproj     --verify-no-changes
dotnet format tests/Aivis.Tests.csproj --verify-no-changes

# ドキュメント生成
docfx ./docfx.json    # _site/ へ出力
docfx serve _site     # ローカルプレビュー
```

ライブラリは .NET 6.0 / 7.0 / 8.0 / 9.0 / 10.0 をターゲットにしています。
5 つすべての SDK が必要です（`mise install` で導入できます）。テストは .NET 9.0 で実行します。

## 作業のルール

- **変更後は必ず `dotnet build` と `dotnet test` を実行してください。** CI は
  フォーマット検証・ビルド・テスト・全サンプルのビルドを行うため、いずれかが崩れると失敗します
- **PR を出す前に `dotnet format` を実行してください。** CI が `--verify-no-changes` で検証します
- **サンプルプロジェクトも壊さないでください。** `src/` の公開 API を変更したら、
  `sample/` 配下の 5 つのプロジェクトがビルドできるか確認します
- **API キーをコミットしないでください。** テストやサンプルでは環境変数やコンソール入力から受け取ります
- **実際の API を呼ぶテストを追加しないでください。** `tests/MockHttpClientProvider.cs` を使います
- 破壊的変更を伴う場合は `CHANGELOG.md` と `Aivis.Net.targets` の `PackageReleaseNotes` の両方を更新します

## コーディング規約

`.editorconfig` に定義があり、`dotnet format` で適用できます。

- インデントは半角スペース 4、改行は CRLF
- `using` はグループ化して並び替え（`System` を先頭）
- 型・メンバーは PascalCase、インターフェイスは `I` 接頭辞（`ISpeaker`）
- 非公開フィールドは `_camelCase`、非公開 static は `s_camelCase`、ローカル・引数は `camelCase`
- ブレースは単文でも省略しない。`var` は右辺から型が明白な場合のみ
- 公開メンバーには XML ドキュメントコメントを付ける（DocFX の API リファレンスに反映されます）
- コメント・ドキュメント・例外メッセージは日本語で記述する（既存コードに合わせる）

## 実装パターン

- API 領域ごとにクライアントクラス `Aivis{領域}Client` を作り、`AivisClientOptions` を受け取って
  コンストラクタで `Clone()` した内容を保持します
- HTTP 呼び出しは `HttpHelper` の拡張メソッド（`GetAsync` / `GetWithAuthAsync` / `PostWithAuthAsync`）を使います
- オプションクラスは、値の妥当性をプロパティセッターで検証し、
  流暢に書ける `SetXxx()` メソッド（`this` を返す）を併せて提供します
- レスポンス DTO は `System.Text.Json` でデシリアライズし、
  プロパティ名は `[JsonPropertyName("snake_case")]` で対応付けます

新しいエンドポイントを追加する手順は [docs/architecture.md](./docs/architecture.md) にまとめてあります。

## 注意すべき点

- `TalkToSpeechOptions` のいくつかのプロパティは相互に干渉します
  （`StyleId` と `StyleName` は排他、`OutputFormat` の変更が `OutputBitrate` / `OutputSamplingRate` に波及）。
  変更時は `tests/TalkToSpeechOptionsTests.cs` の期待値も確認してください
- `MP3Speaker` を伴うテストは FFmpeg と OpenAL を必要とします。CI では自動インストールされます
- `AivisClientOptions.ApiKey` は `internal` です。テストからは `InternalsVisibleTo` で参照できます
- 実装が API 仕様に追いついていない箇所（`use_volume_normalizer` 未対応、レート制限ヘッダー名の不一致など）が
  あります。詳細は [docs/architecture.md](./docs/architecture.md) の「実装と API 仕様の差異」を参照してください

## コミットと PR

- コミットメッセージは接頭辞付きの短い命令形（`feat:` / `fix:` / `docs:` / `chore:` /
  `refactor:` / `test:` / `update:` / `upgrade:` / `release:`）
- PR は `.github/pull_request_template.md` に従い、概要・変更点・確認したコマンド・関連 Issue を記載します
- ベースブランチは `main` です
