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

var contents = await ttsClient.SynthesizeWithContentsAsync(modelUuid!.Replace("\n", "").Trim(), text!.Trim());

Console.WriteLine($"File Name         : {contents.ContentDisposition}");
Console.WriteLine($"Billing Mode      : {contents.BillingMode}");
Console.WriteLine($"Character Count   : {contents.CharacterCount}");
if (contents.IsPayAsYouGo)
{
    Console.WriteLine($"Credits Remainig  : {contents.CreditsRemaining}");
    Console.WriteLine($"Credits Used      : {contents.CreditsUsed}");
}
else
{
    Console.WriteLine($"Rate Limit        : {contents.RateLimitRemaining}");
}

Aivis.Speakers.MP3Speaker speaker = new();
await speaker.PlayAsync(contents.AudioStream);
speaker.Dispose();