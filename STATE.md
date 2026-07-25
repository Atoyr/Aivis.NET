# プロジェクトの現状と今後の課題

Aivis.NET の実装状況、既知の不足、今後取り組む項目をまとめたドキュメントです。
着手時はこのファイルの該当項目を確認し、完了したらチェックを更新してください。

最終更新: 2026-07-25 / 対象バージョン: v1.2.1

## 現在の状況

| 項目 | 状態 |
|---|---|
| 対象フレームワーク | .NET 6.0 / 7.0 / 8.0 / 9.0 / 10.0 |
| 実装済み API | TTS 合成、モデル検索・詳細・ダウンロード URL、ユーザー情報 |
| 音声再生 | `MP3Speaker`（FFmpeg + OpenAL）、`ISpeaker` による差し替え |
| CI | フォーマット検証・ビルド・テスト・全 5 サンプルのビルド |
| ドキュメント | DocFX で GitHub Pages に公開 |

---

## 1. 未実装の API エンドポイント

Aivis Cloud API には存在するが、ライブラリが対応していない領域です。
実装時は [OpenAPI 仕様](https://api.aivis-project.com/v1/openapi.json)と
`docs/architecture.md` の「新しいエンドポイントを追加する手順」を参照してください。

### ユーザー辞書 API

読み方を辞書で制御する機能です。`TalkToSpeechOptions.UserDictionaryUuid` は既に
リクエストへ送れる状態ですが、**辞書そのものを作成・管理する手段がない**ため、
現状ではこのオプションを実質的に利用できません。優先度が高い理由はここにあります。

- [ ] `GET /v1/user-dictionaries` — 一覧取得
- [ ] `GET /v1/user-dictionaries/{uuid}` — 取得
- [ ] `PUT /v1/user-dictionaries/{uuid}` — 置換
- [ ] `DELETE /v1/user-dictionaries/{uuid}` — 削除
- [ ] `POST /v1/user-dictionaries/{uuid}/import` — AivisSpeech Engine 形式のインポート
- [ ] `GET /v1/user-dictionaries/{uuid}/export` — AivisSpeech Engine 形式のエクスポート

必要な DTO: `UserDictionaryRequest`（`name` / `description` / `word_properties`）、
`UserDictWord`（`surface` / `yomi` / `pronunciation` / `accent_type` / `mora_count` /
`word_type` / `priority` / 品詞情報）。新規クラスは `AivisUserDictionaryClient` を想定します。

### 課金・使用量 API

- [ ] `GET /v1/payment/subscriptions` — サブスクリプション一覧
- [ ] `GET /v1/payment/credit-transactions` — クレジット取引履歴
- [ ] `GET /v1/payment/api-keys` — API キー一覧
- [ ] `GET /v1/payment/usage-summaries` — 使用量サマリー

必要な DTO: `SubscriptionResponse` / `SubscriptionStatus` / `SubscriptionType`、
`CreditTransactionResponse` / `CreditTransactionKind` / `CreditTransactionType`、
`APIKeyResponse`、`UsageSummaryResponse`。新規クラスは `AivisPaymentClient` を想定します。

---

## 2. 既知の不具合・仕様との差異

実装が API 仕様に追いついていない箇所です。いずれも利用者から見て挙動が誤って見えます。

- [ ] **レート制限ヘッダー名の不一致** — `AivisTTSClient.SynthesizeWithContentsAsync` は
  `X-Aivis-Rate-Limit-Remaining` を読んでいるが、API が返すのは
  `X-Aivis-RateLimit-Requests-Remaining`。結果として `TTSContents.RateLimitRemaining` は常に `0` になる
  （`src/AivisTTSClient.cs`）
- [ ] **レート制限の他ヘッダーが未取得** — `X-Aivis-RateLimit-Requests-Limit` と
  `X-Aivis-RateLimit-Requests-Reset` を `TTSContents` が保持していない。
  上の修正とあわせて `RateLimitLimit` / `RateLimitReset` を追加するのが自然
- [ ] **`use_volume_normalizer` が未対応** — 音量正規化の有効/無効を指定できない。
  `TalkToSpeechOptions` と `Schemas.TTSRequest` の両方に追加が必要
- [ ] **モデル検索の `tiers` が未対応** — `Official` / `Community` での絞り込みができない
  （`src/SearchVoiceModelOptions.cs`）
- [ ] **`TTSContents.ContentDisposition` の名前と中身が食い違う** — 実際に入るのは
  ヘッダー全体ではなく抽出済みのファイル名。`FileName` へのリネームが妥当だが破壊的変更になるため、
  次のメジャーリリースに合わせる

---

## 3. 設計・API 表面の課題

破壊的変更を伴うものが多いため、まとめて次のメジャーバージョンで対応するのが現実的です。

- [ ] **`CancellationToken` を受け取れない** — `ITalkToSpeech` の 3 メソッドと
  `AivisModelClient` / `AivisUsersClient` の全メソッドがキャンセルに対応していない。
  長文合成を中断できず、ASP.NET Core などでリクエスト打ち切りに追従できない。
  既定値付きの引数として追加すれば非破壊で導入可能
- [ ] **`Async` サフィックスの欠落** — `Task` を返す以下のメソッドが .NET の命名規約に反している。
  `AivisModelClient.SearchModels` / `GetModelDetail` / `GetDownloadUrl`、
  `AivisUsersClient.GetMe` / `GetUserInfo`。
  旧名を `[Obsolete]` で残しつつ `Async` 版を追加する移行が望ましい
- [ ] **例外設計が粗い** — `AivisTTSClient` が 402/404/422/429/5xx をすべて
  `NotSupportedException` に丸めており、呼び出し側が種類を判別できない。
  `AivisApiException`（`StatusCode` を保持）のような専用の例外型を導入したい
- [ ] **`HttpClientProvider` の破棄責務が曖昧** — `IDisposable` を実装しているが、
  `AivisClientOptions` も各クライアントも破棄しない。誰が所有するかを明文化するか、
  `IHttpClientFactory` に対応する
- [ ] **XML ドキュメントコメントの欠落** — `AivisUsersClient.GetMe` / `GetUserInfo`、
  `TTSContents`、`MediaType`、`AudioChannel`、`HttpHelper` の公開メンバーにコメントがなく、
  API リファレンスが空になっている
- [ ] **`MP3Speaker` が MP3 固定** — `FFmpegDecodeOptions.InputFormat` が
  `init` 専用かつ既定 `mp3` のため、`OutputFormat` に `wav` や `opus` を指定した音声を
  そのまま再生できない。形式を後から変更できるようにするか、汎用の `AudioSpeaker` を用意したい

---

## 4. テストの不足

現在 34 のテストファイルがありますが、以下は未カバーです。

- [ ] **`AivisModelClient` の単体テストが存在しない** — 検索・詳細取得・ダウンロード URL の
  いずれも未検証。`tests/MockHttpClientProvider.cs` を使って追加する
- [ ] **`SearchVoiceModelsOptions` のテストが存在しない** — `Page` / `Limit` の
  範囲検証や配列プロパティの挙動が未検証
- [ ] **`HttpHelper` のテストが存在しない** — スネークケース変換、配列のクエリ展開、
  `Content-Disposition` からのファイル名抽出はいずれも壊れやすい箇所
- [ ] **エラーレスポンスの網羅性** — `AivisTTSClient` の各ステータスコードに対する
  例外変換が部分的にしか検証されていない

---

## 5. ドキュメント・ビルド環境

- [ ] **DocFX が AI 向けドキュメントまで公開している** — `docfx.json` の content 指定が
  `**/*.{md,yml}` のため、`CLAUDE.md` や `AGENTS.md` が
  GitHub Pages に公開される（`_site/CLAUDE.html` が生成済み）。
  `CONTRIBUTING.md` などを意図的に載せる一方で、AI 向けファイルは exclude するのが望ましい
- [ ] **生成物に削除済みクラスが残っている** — `api/Aivis.Heartbeat.yml` と
  `_site/api/Aivis.Heartbeat.html` は `src/` に存在しない `Heartbeat` クラスのもの。
  `api/` を一度クリアしてから再生成する必要がある
- [ ] **`_site/` と `api/` をコミットする運用の見直し** — 差分が大きくレビューを妨げている。
  GitHub Pages のデプロイは `.github/workflows/docs.yml` で行っているため、
  生成物を追跡対象から外せる可能性がある
- [ ] **英語ドキュメントの整備** — 現在まとまった英語の記述は `NUGET_README.md` のみ。
  README とガイドの英語版があると利用者層が広がる
- [ ] **CHANGELOG の二重管理** — `CHANGELOG.md` と `Aivis.Net.targets` の
  `PackageReleaseNotes` に同じ内容を手で書いている。生成に寄せる余地がある

---

## 6. 優先度

| 優先度 | 項目 |
|---|---|
| 高 | レート制限ヘッダーの不一致修正（2 章）／`AivisModelClient` のテスト追加（4 章）／`use_volume_normalizer` 対応（2 章） |
| 中 | ユーザー辞書 API の実装（1 章）／`CancellationToken` 対応（3 章）／XML コメントの補完（3 章）／DocFX の公開範囲整理（5 章） |
| 低 | 課金 API の実装（1 章）／例外型の再設計（3 章）／`Async` サフィックス移行（3 章）／英語ドキュメント（5 章） |

「高」はいずれも非破壊的な修正で、利用者から見た不具合を直接解消します。
「低」の多くは破壊的変更を含むため、v2.0 としてまとめて扱うことを想定しています。

---

## 関連ドキュメント

| ファイル | 内容 |
|---|---|
| `CLAUDE.md` | API 仕様とエンドポイントの実装状況 |
| `AGENTS.md` | 作業手順・コマンド・コーディング規約 |
| `docs/architecture.md` | 内部構造、新しいエンドポイントを追加する手順 |
| `CONTRIBUTING.md` | 貢献手順とリリース手順 |
| `CHANGELOG.md` | リリースごとの変更履歴 |
