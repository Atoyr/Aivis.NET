---
name: 🐛 バグ報告 / Bug Report
description: ライブラリの不具合を報告するためのテンプレート
title: "[Bug] "
labels: [Bug]
assignees: ''
---

body:
  - type: markdown
    attributes:
      value: |
        **バグの報告ありがとうございます！以下の内容をできるだけ詳しく記入してください。**

  - type: input
    id: version
    attributes:
      label: ライブラリのバージョン
      placeholder: 例）v1.2.3
    validations:
      required: true

  - type: textarea
    id: description
    attributes:
      label: バグの内容
      description: どんな問題が起きているか、何を期待していたかを詳しく書いてください。
    validations:
      required: true

  - type: textarea
    id: repro
    attributes:
      label: 再現手順
      description: バグを再現するためのコード、設定、環境などを記載してください。
      placeholder: |
        1. ライブラリを初期化
        2. `DoSomething()` を呼び出す
        3. 例外が発生
    validations:
      required: false

  - type: input
    id: environment
    attributes:
      label: 使用環境
      description: OS・.NETのバージョンなど
      placeholder: Windows 11, .NET 8
    validations:
      required: false

