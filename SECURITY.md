# セキュリティポリシー

## サポート対象バージョン

セキュリティ修正は、NuGet で公開されている最新のマイナーバージョン系列に対して提供します。

| バージョン | サポート状況 |
|---|---|
| 1.2.x | ✅ サポート対象 |
| 1.1.x | ⚠️ 重大な脆弱性のみ対応 |
| 1.0.x 以前 | ❌ サポート対象外 |

## 脆弱性の報告

**セキュリティ上の問題は、公開の Issue では報告しないでください。**

以下のいずれかの方法で非公開に報告してください。

1. GitHub の [Security Advisories](https://github.com/Atoyr/Aivis.NET/security/advisories/new) から報告する（推奨）
2. [atoyr80118891@gmail.com](mailto:atoyr80118891@gmail.com) にメールを送る

### 報告に含めていただきたい情報

- 影響を受けるバージョン
- 脆弱性の種類と概要
- 再現手順（可能であれば最小の再現コード）
- 想定される影響範囲
- 回避策を把握していればその内容

### 対応の流れ

| 段階 | 目安 |
|---|---|
| 受領の連絡 | 3 営業日以内 |
| 影響範囲の一次評価 | 7 営業日以内 |
| 修正版のリリース | 深刻度に応じて調整のうえ、報告者に通知 |

個人が管理する非営利のプロジェクトのため、対応が遅れる場合があります。あらかじめご了承ください。
修正の公開時には、希望される場合に限り報告者を謝辞に記載します。

## 利用者向けの注意事項

### API キーの取り扱い

`AivisClientOptions` に渡す API キーは、Aivis Cloud API の課金に直結する認証情報です。

- **ソースコードや設定ファイルにハードコードしない**でください
- 環境変数、[User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets)、シークレット管理サービスなどから注入してください
- クライアントサイド（デスクトップアプリ・モバイルアプリ・ブラウザ）に API キーを埋め込むと、利用者が抽出できてしまいます。自前のバックエンド経由で呼び出す構成を検討してください

```csharp
// 推奨: 環境変数から読み込む
var apiKey = Environment.GetEnvironmentVariable("AIVIS_API_KEY")
    ?? throw new InvalidOperationException("AIVIS_API_KEY が設定されていません。");
var options = new AivisClientOptions(apiKey);
```

### 外部プロセスの実行

`MP3Speaker` は音声のデコードに外部プロセスとして FFmpeg を起動します。
`FFmpegDecodeOptions.SetFFmpegPath` および `SetExtraInputArgs` / `SetExtraOutputArgs` に
外部入力由来の値をそのまま渡すと、意図しないコマンドが実行される恐れがあります。
これらの値はアプリケーション側で固定するか、厳格に検証してください。

### 合成テキストの取り扱い

`TalkToSpeechOptions.UseSsml` は既定で `true` です。
利用者の入力をそのまま合成テキストとして渡す場合、`<` や `&` などが SSML タグとして解釈されます。
プレーンテキストとして扱いたい場合は `SetUseSsml(false)` を指定するか、XML エスケープを行ってください。

## 対象外の報告

以下はこのリポジトリのセキュリティポリシーの対象外です。

- Aivis Cloud API（`https://api.aivis-project.com`）自体の脆弱性
  → [Aivis Project](https://aivis-project.com) に直接報告してください
- FFmpeg / OpenAL / OpenTK など、依存する第三者ソフトウェアの脆弱性
  → 各プロジェクトに報告してください（このライブラリ側の対応が必要な場合は Issue でお知らせください）
