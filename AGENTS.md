# Repository Guidelines

## プロジェクト構成とモジュール
- `src/`: ライブラリ本体（`Aivis.Net.csproj`）。`Schemas/`（DTO・Enum）と`Speakers/`（音声再生）を中心に配置。
- `tests/`: xUnit テスト（`Aivis.Tests.csproj`）。`src/`構成に対応し、`*Tests.cs`を命名規則とします。
- `sample/`: 使い方の最小サンプル（`StreamPlay`、`NAudio` など）。
- `docs/`・`api/`・`_site/`: DocFX 用ソースと生成物。
- `.github/`: CI ワークフロー、Issue/PR テンプレート。

## ビルド・テスト・開発コマンド
- 依存取得: `dotnet restore src/Aivis.Net.csproj`
- ビルド(Release): `dotnet build src/Aivis.Net.csproj -c Release --no-restore`
- テスト: `dotnet test tests/Aivis.Tests.csproj -c Release --no-build`
- カバレッジ: `dotnet test tests/Aivis.Tests.csproj --collect:"XPlat Code Coverage"`
- フォーマット: `dotnet format`（CIで検証。PR前にローカル実行してください）
- サンプル: `dotnet build sample/StreamPlay/Aivis.Sample.StreamPlay.csproj -c Release`

## DocFX ドキュメント
- ツール導入: `dotnet tool update -g docfx`（未導入なら `install`）。
- 生成: `docfx ./docfx.json`（出力は `_site/`）。
- プレビュー: `docfx serve _site`（ローカル閲覧）。
- GitHub Pages 反映は `.github/workflows/docs.yml` により自動化。

## サンプルの実行
- StreamPlay: `dotnet run -c Release -p sample/StreamPlay/Aivis.Sample.StreamPlay.csproj`
  - 実行時に API キーとモデル UUID をコンソール入力。FFmpeg/OpenAL が必要。
- NAudio(Windows): `dotnet run -c Release -p sample/NAudio/Aivis.Sample.NAudio.csproj`
  - OpenAL は不要。NAudio を利用した再生例。

## コーディング規約・命名
- インデント: スペース4 / 改行: CRLF（`.editorconfig` 準拠）。
- using: グループ化＋並び替え（`System` を先頭）。
- 命名: 型/メンバーは PascalCase、インターフェイスは `I` 接頭（例: `ISpeaker`）。
- フィールド: 非公開は `_camelCase`、非公開 static は `s_camelCase`、ローカル/引数は `camelCase`。
- ブレース必須、初期化子・式の簡素化を推奨。`var`は型が明白な場合のみ。

## テスト方針
- フレームワーク: xUnit、モック: Moq、カバレッジ: `coverlet.collector`。
- 配置/命名: `tests/` 配下に `*Tests.cs`、`[Fact]`/`[Theory]` を使用。
- 音声系テスト: FFmpeg と OpenAL が必要（CI は自動インストール）。ローカル実行時は事前に導入してください。

## コミット／PR ガイドライン
- コミット: 目的を明確に（例: `fix:`, `feat:`, `chore:`, `update:`, `upgrade:`, `release:`）。短く命令形で書き、範囲を限定。
- PR: `.github/pull_request_template.md` に従い、概要・変更点・チェックリスト（`dotnet build`/`dotnet test` 実行）・関連 Issue（例: `Fix #123`）を記載。必要に応じてドキュメントやスクリーンショットを追加。

## セキュリティ／設定の注意
- API キー等の秘密情報はコミットしない。`AivisClientOptions` へは環境変数やユーザーシークレットで注入。
- ローカルでの音声再生は FFmpeg/OpenAL を事前に準備。サンプルを参照して利用パターンを確認。
