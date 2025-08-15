# Aivis.NET Project

## Overview
Aivis.NET is an unofficial C# library for the Aivis API (https://aivis-project.com), providing Text-to-Speech functionality.

## Project Structure
- `src/` - Main library source code (Aivis.Net.csproj)
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
- .NET 8.0
- .NET 9.0

## Key Dependencies
- OpenTK 4.9.4 (for audio playback with OpenAL)
- System.Text.Json (for JSON serialization)

## Build Commands
```bash
dotnet build
dotnet test
dotnet pack
```

## Main Classes
- `AivisTTSClient` - Main TTS client for text-to-speech functionality
- `AivisClientOptions` - Configuration options for API clients
- `AivisModelClient` - Client for model-related operations
- `AivisUsersClient` - Client for user-related operations
- `TTSContents` - Response with audio stream and metadata
- `MP3Speaker` - Audio playback using FFmpeg and OpenAL
- `ISpeaker` - Interface for audio playback implementations

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