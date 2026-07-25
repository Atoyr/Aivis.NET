# Aivis.NET Project

## Overview
Aivis.NET is an unofficial C# library for the Aivis Cloud API (https://api.aivis-project.com), providing Text-to-Speech functionality.

- API base URL: `https://api.aivis-project.com`
- API reference: https://api.aivis-project.com/v1/docs (OpenAPI spec: https://api.aivis-project.com/v1/openapi.json)
- Authentication: `Authorization: Bearer <API_KEY>` (Bearer token)

## Related Documents
このファイルはプロジェクト概要と API 仕様を扱います。作業内容に応じて次も参照してください。

| Document | 内容 |
|---|---|
| `AGENTS.md` | 作業手順・コマンド・コーディング規約・注意点（**コード変更前に必ず読むこと**） |
| `STATE.md` | 未実装機能、既知の不具合、今後の課題と優先度（**何に着手するか決める前に読むこと**） |
| `docs/architecture.md` | 内部構造、処理の流れ、拡張ポイント、実装と API 仕様の差異 |
| `CONTRIBUTING.md` | 人間向けの貢献手順とリリース手順 |
| `SECURITY.md` | API キーの取り扱いを含むセキュリティ方針 |

## Project Structure
- `src/` - Main library source code (Aivis.Net.csproj)
  - `Schemas/` - API request/response DTOs and enums
  - `Speakers/` - Audio playback implementations (FFmpeg + OpenAL)
- `sample/` - Sample applications:
  - `Contents/` - Basic TTS contents example
  - `NAudio/` - NAudio speaker implementation example
  - `SearchModels/` - Model search example
  - `StreamPlay/` - Streaming audio playback example
  - `Users/` - User API example
- `tests/` - Unit tests (Aivis.Tests.csproj)
- `docs/` - DocFX user guides (Markdown) and logo assets (SVG / PNG)
- `api/`, `_site/` - DocFX generated output (committed to the repository)
- `nupkg/` - NuGet package output

## Target Frameworks
- .NET 6.0 / 7.0 / 8.0 / 9.0 / 10.0 (library)
- Tests run on .NET 9.0

## Key Dependencies
- OpenTK 4.9.4 (for audio playback with OpenAL)
- System.Text.Json (for JSON serialization)

## Build Commands
```bash
dotnet build   src/Aivis.Net.csproj  -c Release
dotnet test    tests/Aivis.Tests.csproj -c Release
dotnet format  src/Aivis.Net.csproj  --verify-no-changes   # CI と同じ書式検証
dotnet pack    src/Aivis.Net.csproj  -c Release
```
CI で実行される全コマンドとサンプルの実行方法は `AGENTS.md` を参照してください。

## Aivis Cloud API Specification (v1)

### Endpoint Coverage

| Endpoint | Method | Summary | Implemented in |
|---|---|---|---|
| `/v1/tts/synthesize` | POST | テキストから音声を合成する | `AivisTTSClient` |
| `/v1/aivm-models/search` | GET | 公開されている音声合成モデルを検索する | `AivisModelClient.SearchModels` |
| `/v1/aivm-models/{aivm_model_uuid}` | GET | モデルの詳細情報を取得する | `AivisModelClient.GetModelDetail` |
| `/v1/aivm-models/{aivm_model_uuid}/download` | GET | モデルファイルをダウンロードする | `AivisModelClient.GetDownloadUrl` |
| `/v1/users/me` | GET | ログイン中ユーザーのアカウント情報を取得する | `AivisUsersClient.GetMe` |
| `/v1/users/{handle}` | GET | 指定ユーザーのアカウント情報を取得する | `AivisUsersClient.GetUserInfo` |
| `/v1/user-dictionaries` | GET | ユーザー辞書の一覧を取得する | **未実装** |
| `/v1/user-dictionaries/{uuid}` | GET / PUT / DELETE | ユーザー辞書の取得・置換・削除 | **未実装** |
| `/v1/user-dictionaries/{uuid}/import` | POST | AivisSpeech Engine の辞書データをインポートする | **未実装** |
| `/v1/user-dictionaries/{uuid}/export` | GET | AivisSpeech Engine 向け辞書データをエクスポートする | **未実装** |
| `/v1/payment/subscriptions` | GET | サブスクリプション一覧を取得する | **未実装** |
| `/v1/payment/credit-transactions` | GET | クレジット取引履歴を取得する | **未実装** |
| `/v1/payment/api-keys` | GET | API キー一覧を取得する | **未実装** |
| `/v1/payment/usage-summaries` | GET | API 使用量サマリーを取得する | **未実装** |

### TTS Synthesize (`POST /v1/tts/synthesize`)

Request body (`TTSRequest`)。「Aivis.NET」列は対応する `TalkToSpeechOptions` のプロパティです。

| Field | Type | Default | Aivis.NET | Notes |
|---|---|---|---|---|
| `model_uuid` | string (UUID) | required | `SynthesizeXxxAsync` の第 1 引数 | 音声合成モデルの UUID |
| `speaker_uuid` | string (UUID) | - | `SpeakerUuid` | マルチスピーカーモデル時の話者指定 |
| `style_id` | int | - | `StyleId` (0〜31) | スタイル ID(`style_name` と排他) |
| `style_name` | string | - | `StyleName` | スタイル名(`style_id` と排他) |
| `user_dictionary_uuid` | string (UUID) | - | `UserDictionaryUuid` | 適用するユーザー辞書 |
| `text` | string | required | `SynthesizeXxxAsync` の第 2 引数 | 合成するテキスト |
| `use_ssml` | bool | `true` | `UseSsml` | SSML 解釈の有効化 |
| `use_volume_normalizer` | bool | `true` | **未実装** | 音量正規化(ラウドネスノーマライズ)。常に API 側の既定値が適用される |
| `language` | string | `ja` | `Language` | 現在は `ja` のみ |
| `speaking_rate` | number | `1.0` | `SpeakingRate` (0.5〜2.0) | 話速 |
| `emotional_intensity` | number | `1.0` | `EmotionIntensity` (0.0〜2.0) | 感情表現の強さ |
| `tempo_dynamics` | number | `1.0` | `TempoDynamics` (0.0〜2.0) | テンポの緩急 |
| `pitch` | number | `0.0` | `Pitch` (-1.0〜1.0) | ピッチ |
| `volume` | number | `1.0` | `Volume` (0.0〜2.0) | 音量 |
| `leading_silence_seconds` | number | `0.1` | `LeadingSilenceSeconds` (0.0〜60.0) | 先頭の無音 |
| `trailing_silence_seconds` | number | `0.1` | `TrailingSilenceSeconds` (0.0〜60.0) | 末尾の無音 |
| `line_break_silence_seconds` | number | `0.4` | `LineBreakSilenceSeconds` (0.0〜60.0) | 改行ごとの無音 |
| `output_format` | enum | `mp3` | `OutputFormat` (`MediaType`) | `wav` / `flac` / `mp3` / `aac` / `opus` |
| `output_bitrate` | int | - | `OutputBitrate` (8〜320) | mp3/aac/opus 時のビットレート (kbps) |
| `output_sampling_rate` | int | `44100` | `OutputSamplingRate` | 出力サンプリングレート |
| `output_audio_channels` | enum | `mono` | `OutputAudioChannels` (`AudioChannel`) | `mono` / `stereo` |

Response: 音声データのバイナリストリーム(チャンク転送、ストリーミング再生可能)。

Response headers (billing metadata):

| Header | `TTSContents` | 備考 |
|---|---|---|
| `X-Aivis-Billing-Mode` | `BillingMode` | `PayAsYouGo` / `Subscription`。`IsPayAsYouGo` / `IsSubscription` で判定可能 |
| `X-Aivis-Character-Count` | `CharacterCount` | 消費された文字数 |
| `X-Aivis-Credits-Used` | `CreditsUsed` | 従量課金時 |
| `X-Aivis-Credits-Remaining` | `CreditsRemaining` | 従量課金時 |
| `X-Aivis-RateLimit-Requests-Remaining` | `RateLimitRemaining` | 定額プラン時。**実装は `X-Aivis-Rate-Limit-Remaining` を読んでおり値が一致しない** |
| `X-Aivis-RateLimit-Requests-Limit` / `-Reset` | **未取得** | 定額プラン時 |

Error status codes: `401` (無効な API キー), `402` (クレジット残高不足), `404` (モデルが見つからない), `422` (パラメータ不正), `429` (レート制限), `500`/`502`/`503`/`504` (音声合成サーバー Citoras 側のエラー)。

### Model Search (`GET /v1/aivm-models/search`)

Query parameters: `keyword`, `tags`, `tiers` (`Official` / `Community`), `categories`, `voice_timbres`, `license_types`, `sort` (default: `download`), `page` (default: 1), `limit` (default: 24)。

`SearchVoiceModelsOptions` が対応しているのは `keyword` / `tags` / `categories` / `voice_timbres` /
`license_types`(`LicenceTypes`) / `sort` / `page` / `limit` です。**`tiers` は未実装**。
`Limit` は 1〜30、`Page` は 1 以上でセッターが検証します。

### User Dictionaries (未実装 — 実装時の参考)

- `UserDictionaryRequest`: `name`, `description`, `word_properties`
- `UserDictWord`: `surface`, `yomi`, `pronunciation`, `accent_type`, `mora_count`, `word_type`, `priority`, 品詞情報など(AivisSpeech Engine 互換)
- import/export により AivisSpeech Engine と辞書データを相互運用できる

### Payment API (未実装 — 実装時の参考)

- `SubscriptionResponse` / `SubscriptionStatus` / `SubscriptionType`
- `CreditTransactionResponse` / `CreditTransactionKind` / `CreditTransactionType`
- `APIKeyResponse`, `UsageSummaryResponse`

## Main Classes
- `AivisTTSClient` - Main TTS client (`SynthesizeAsync` → `byte[]`, `SynthesizeStreamAsync` → `Stream`, `SynthesizeWithContentsAsync` → `TTSContents`)
- `AivisClientOptions` - Configuration options for API clients (API key, `BaseUrl`, `HttpClientProvider`)
- `AivisModelClient` - Client for model search / detail / download URL
- `AivisUsersClient` - Client for user-related operations
- `TalkToSpeechOptions` - TTS request options mapped to `TTSRequest` fields
- `SearchVoiceModelsOptions` - Query options for model search
- `TTSContents` - Response with audio stream and billing metadata (`X-Aivis-*` headers, `IsPayAsYouGo` / `IsSubscription`)
- `IHttpClientProvider` / `HttpClientProvider` - `HttpClient` の供給元。テストでは `tests/MockHttpClientProvider.cs` を差し込む
- `HttpHelper` - URL 構築とリクエスト送信の拡張メソッド (`GetAsync` / `GetWithAuthAsync` / `PostWithAuthAsync`)
- `MediaType` / `AudioChannel` - 出力形式・チャンネル数の enum (`ToFormatString()` で API 表現へ変換)
- `Schemas/` - DTOs mirroring OpenAPI component schemas (`AivmModelResponse`, `UserResponse`, enums, etc.)
- `ISpeaker` - Interface for audio playback implementations
- `MP3Speaker` - Audio playback using FFmpeg and OpenAL (`IDisposable`)
- `FFmpegDecodeOptions` - FFmpeg のデコード設定
- `Speakers.Device` - OpenAL の再生デバイス列挙

## Conventions
- API schema DTOs live in `src/Schemas/` and mirror the OpenAPI component schema names (e.g. `AivmModelResponse`, `UserResponseForMe`)
- JSON serialization uses `System.Text.Json`; string enums use `EnumMemberStringEnumConverter`
- Each API area gets its own client class (`Aivis{Area}Client`) that takes `AivisClientOptions` and uses `IHttpClientProvider`
- オプションクラスは値の検証をプロパティセッターで行い、`this` を返す `SetXxx()` メソッドを併せて提供する
- 新しいエンドポイントを実装する場合は、上記の Endpoint Coverage 表の「未実装」項目と OpenAPI 仕様 (https://api.aivis-project.com/v1/openapi.json) を参照すること。実装手順は `docs/architecture.md` にまとめてある

## Audio Playback Requirements
- FFmpeg (for audio decoding)
- OpenAL (for audio playback)
- Alternative: NAudio for Windows-only scenarios (see sample)

## Usage Examples

### Streaming TTS with MP3Speaker
```csharp
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);

using Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(stream);
```
`MP3Speaker` は `IDisposable` です。FFmpeg プロセスと OpenAL リソースを解放するため必ず破棄してください。

### Using NAudio (Windows)
```csharp
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);

// Custom NAudio speaker implementation (see sample/NAudio)
Aivis.Speakers.ISpeaker speaker = new Aivis.Sample.NAudio.NAudioSpeaker();
await speaker.PlayAsync(stream);
```
