name: ❓ 質問 / Question
description: ライブラリの使い方などに関する質問はこちら
title: "[Question] "
labels: [question]
assignees: ''

body:
  - type: textarea
    id: question
    attributes:
      label: 質問の内容
      description: ライブラリのどの機能について、どのような質問かを詳しく書いてください。
    validations:
      required: true

  - type: input
    id: version
    attributes:
      label: ライブラリのバージョン（わかれば）
      placeholder: 例）v1.2.3

