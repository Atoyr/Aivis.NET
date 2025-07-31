Console.WriteLine("input api key");
var apiKey = Console.ReadLine();

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("api key is invalid");
    return;
}

Console.WriteLine("input model uuid");
var uuid = Console.ReadLine();

Console.WriteLine("input text");
var text = Console.ReadLine();

Aivis.AivisClientOptions options = new(apiKey!.Trim());
Aivis.AivisTTLClient ttlCient = new(options);
var stream = await ttlCient.Synthesize(uuid!.Replace("\n", "").Trim(), text);

Aivis.Speaker speaker = new();
await speaker.PlayAsync(Aivis.MediaType.MP3, stream);