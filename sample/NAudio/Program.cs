string? apiKey;
string? modelUuid;
string? text;

Console.WriteLine("input api key");
while (string.IsNullOrWhiteSpace(apiKey = Console.ReadLine()))
{
    Console.WriteLine("api key is invalid");
}

Console.WriteLine("input model uuid");
while (string.IsNullOrWhiteSpace(modelUuid = Console.ReadLine()))
{
    Console.WriteLine("model uuid is invalid");
}

Console.WriteLine("input text");
while (string.IsNullOrWhiteSpace(text = Console.ReadLine()))
{
    Console.WriteLine("text is invalid");
}

Aivis.AivisClientOptions options = new(apiKey!.Trim());
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid!.Replace("\n", "").Trim(), text!);

Aivis.Speakers.ISpeaker speaker = new Aivis.Sample.NAudio.NAudioSpeaker();
Console.WriteLine("チャンクの受信を開始します...");
Console.WriteLine("ストリーミング再生を開始します... ");
await speaker.PlayAsync(stream);