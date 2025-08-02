# Aivis.NET Project

## Overview
Aivis.NET is an unofficial C# library for the Aivis API (https://aivis-project.com), providing Text-to-Speech functionality.

## Project Structure
- `src/` - Main library source code (Aivis.Net.csproj)
- `sample/Console/` - Console application example
- `tests/` - Unit tests
- `docs/` - Logo assets
- `nupkg/` - NuGet package output

## Target Frameworks
- .NET 8.0
- .NET 9.0

## Key Dependencies
- NAudio 2.2.1 (for audio playback)

## Build Commands
```bash
dotnet build
dotnet test
dotnet pack
```

## Main Classes
- `AivisTTSClient` - Main TTS client
- `AivisClientOptions` - Configuration options
- `Speaker` - Audio playback functionality
- `TTSContents` - Response with audio data and metadata

## Usage Example
```csharp
Aivis.AivisClientOptions options = new(apiKey);
Aivis.AivisTTSClient ttsClient = new(options);
var contents = await ttsClient.SynthesizeWithContentsAsync(modelUuid, text);

Aivis.Speaker speaker = new();
await speaker.PlayAsync(Aivis.MediaType.MP3, contents.Audio);
```