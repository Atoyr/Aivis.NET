# Aivis.NET Project

## Overview
Aivis.NET is an unofficial C# library for the Aivis Cloud API (https://api.aivis-project.com), providing Text-to-Speech functionality.

- API base URL: `https://api.aivis-project.com`
- API reference: https://api.aivis-project.com/v1/docs (OpenAPI spec: https://api.aivis-project.com/v1/openapi.json)
- Authentication: `Authorization: Bearer <API_KEY>` (Bearer token)

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
- `docs/` - Logo assets (SVG files)
- `nupkg/` - NuGet package output

## Target Frameworks
- .NET 6.0 / 7.0 / 8.0 / 9.0 / 10.0 (library)
- Tests run on .NET 9.0

## Key Dependencies
- OpenTK 4.9.4 (for audio playback with OpenAL)
- System.Text.Json (for JSON serialization)

## Build Commands
```bash
dotnet build
dotnet test
dotnet pack
```

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

Request body (`TTSRequest`):

| Field | Type | Default | Notes |
|---|---|---|---|
| `model_uuid` | string (UUID) | required | 音声合成モデルの UUID |
| `speaker_uuid` | string (UUID) | - | マルチスピーカーモデル時の話者指定 |
| `style_id` | int | - | スタイル ID(`style_name` と排他) |
| `style_name` | string | - | スタイル名(`style_id` と排他) |
| `user_dictionary_uuid` | string (UUID) | - | 適用するユーザー辞書 |
| `text` | string | required | 合成するテキスト |
| `use_ssml` | bool | `true` | SSML 解釈の有効化 |
| `use_volume_normalizer` | bool | `true` | 音量正規化(ラウドネスノーマライズ) |
| `language` | string | `ja` | 現在は `ja` のみ |
| `speaking_rate` | number | `1.0` | 話速 |
| `emotional_intensity` | number | `1.0` | 感情表現の強さ |
| `tempo_dynamics` | number | `1.0` | テンポの緩急 |
| `pitch` | number | `0.0` | ピッチ |
| `volume` | number | `1.0` | 音量 |
| `leading_silence_seconds` | number | `0.1` | 先頭の無音 |
| `trailing_silence_seconds` | number | `0.1` | 末尾の無音 |
| `line_break_silence_seconds` | number | `0.4` | 改行ごとの無音 |
| `output_format` | enum | `mp3` | `wav` / `flac` / `mp3` / `aac` / `opus` |
| `output_bitrate` | int | - | mp3/aac/opus 時のビットレート (kbps) |
| `output_sampling_rate` | int | `44100` | 出力サンプリングレート |
| `output_audio_channels` | enum | `mono` | `mono` / `stereo` |

Response: 音声データのバイナリストリーム(チャンク転送、ストリーミング再生可能)。

Response headers (billing metadata, `TTSContents` で取得可能):
- `X-Aivis-Billing-Mode` - 課金モード (`PayAsYouGo` / `Subscription`)
- `X-Aivis-Character-Count` - 消費された文字数
- 従量課金時: `X-Aivis-Credits-Used` / `X-Aivis-Credits-Remaining`
- 定額プラン時: `X-Aivis-RateLimit-Requests-Limit` / `-Remaining` / `-Reset`

Error status codes: `401` (無効な API キー), `402` (クレジット残高不足), `404` (モデルが見つからない), `422` (パラメータ不正), `429` (レート制限), `500`/`502`/`503`/`504` (音声合成サーバー Citoras 側のエラー)。

### Model Search (`GET /v1/aivm-models/search`)

Query parameters: `keyword`, `tags`, `tiers` (`Official` / `Community`), `categories`, `voice_timbres`, `license_types`, `sort` (default: `download`), `page` (default: 1), `limit` (default: 24)。

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
- `AivisClientOptions` - Configuration options for API clients (API key, `HttpClientProvider`)
- `AivisModelClient` - Client for model search / detail / download URL
- `AivisUsersClient` - Client for user-related operations
- `TalkToSpeechOptions` - TTS request options mapped to `TTSRequest` fields
- `TTSContents` - Response with audio stream and billing metadata (`X-Aivis-*` headers, `IsPayAsYouGo` / `IsSubscription`)
- `Schemas/` - DTOs mirroring OpenAPI component schemas (`AivmModelResponse`, `UserResponse`, enums, etc.)
- `MP3Speaker` - Audio playback using FFmpeg and OpenAL
- `ISpeaker` - Interface for audio playback implementations

## Conventions
- API schema DTOs live in `src/Schemas/` and mirror the OpenAPI component schema names (e.g. `AivmModelResponse`, `UserResponseForMe`)
- JSON serialization uses `System.Text.Json`; string enums use `EnumMemberStringEnumConverter`
- Each API area gets its own client class (`Aivis{Area}Client`) that takes `AivisClientOptions` and uses `IHttpClientProvider`
- 新しいエンドポイントを実装する場合は、上記の Endpoint Coverage 表の「未実装」項目と OpenAPI 仕様 (https://api.aivis-project.com/v1/openapi.json) を参照すること

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

Aivis.Speakers.ISpeaker speaker = new Aivis.Speakers.MP3Speaker();
await speaker.PlayAsync(stream);
```

### Using NAudio (Windows)
```csharp
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid, text);

// Custom NAudio speaker implementation (see sample/NAudio)
Aivis.Speakers.ISpeaker speaker = new Aivis.Sample.NAudio.NAudioSpeaker();
await speaker.PlayAsync(stream);
```
