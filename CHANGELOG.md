# Changelog
このプロジェクトに対するすべての注目すべき変更はこのファイルに記録されます。

## Unreleased
Added

新機能をここに記載

Changed

変更点をここに記載

Deprecated

非推奨となった機能をここに記載

Removed

削除された機能をここに記載

Fixed

バグ修正をここに記載

Security

セキュリティ修正をここに記載

## 0.5.0 - 2025-08-15
### Added
- `FFmpeg`と`OpenAL`を使用した音声再生の仕組みを追加しました

### Changed
- `TTSContents`の`audio`を`Stream audioStream`に変更しました

### Removed
- NAudioを利用したSpeaker機能は廃止しました
  - NAudioを利用する場合はサンプルアプリを参照してください
- `TextToSpeech`で`byte[]`を戻り値とするメソッドを廃止しました
  - `Stream`を返す機能で利用してください

## 0.4.0 - 2025-08-08
### Added
- モデル検索機能を追加しました


## 0.3.0 - 2025-08-03
### Added
- ストリーミング再生に対応しました

