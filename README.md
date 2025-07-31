<div align="center">

  ![Aivis.NET Logo](/docs/logo_dark.svg#gh-light-mode-only)
  ![Aivis.NET Logo](/docs/logo_light.svg#gh-dark-mode-only)

  <br/>

  <a href="https://www.nuget.org/packages/Aivis.Net">
    <img alt="NuGet Version" src="https://img.shields.io/nuget/v/Aivis.Net">
  </a>

  #

</div>
Aivis.NET は Aivis APIの非公式ライブラリです。(https://aivis-project.com)

# Getting Start.

## 対応バージョン
`.NET 8.0`
`.NET 9.0`

## パッケージの追加


``` 
dotnet add package Aivis.Net
```


## 使用例

Text-to-Speech
``` C#
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
var stream = await ttsClient.Synthesize(modelUuid, text);

Aivis.Speaker speaker = new();
await speaker.PlayAsync(Aivis.MediaType.MP3, stream);
```

# ライセンス
MIT
