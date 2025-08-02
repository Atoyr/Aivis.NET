Console.WriteLine("input api key");
string? apiKey;
string? modelUuid;
string? text;

while (string.IsNullOrWhiteSpace(apiKey = Console.ReadLine()))
{
    Console.WriteLine("api key is invalid");
}

while (string.IsNullOrWhiteSpace(modelUuid = Console.ReadLine()))
{
    Console.WriteLine("model uuid is invalid");
}

while (string.IsNullOrWhiteSpace(text = Console.ReadLine()))
{
    Console.WriteLine("text is invalid");
}

Aivis.AivisClientOptions options = new(apiKey!.Trim());
Aivis.AivisTTSClient ttsCient = new(options);
using var stream = await ttsCient.SynthesizeStreamAsync(modelUuid!.Replace("\n", "").Trim(), text!);

Aivis.Speaker speaker = new();
Console.WriteLine("チャンクの受信を開始します...");
Console.WriteLine("ストリーミング再生を開始します... ");
await speaker.PlayAsync(Aivis.MediaType.MP3, stream);