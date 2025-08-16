Console.WriteLine("input api key");
string? apiKey;
string? modelUuid;
string text = @"エンコードが完了した音声データは順番に、API レスポンスとしてリアルタイムにストリーミング配信します。
このリアルタイム配信を活用すれば、すべての音声合成が完了するのを待たずに、クライアント側で再生を開始できます。
届いた音声データのチャンクを順次デコーダーに送信するよう実装することで、音声の生成と再生を同時並行で行えます。
クライアント側の実装次第となりますが、長いテキストを読み上げる際も最初から最後まで待たずにすぐに再生を開始でき、生成から再生までの待機時間を大幅に短縮できます。
<sub alias=""いちぎょう"">一行</sub>に長いテキストをすべて書くと、その行全体が一度に音声合成されるため、ストリーミング配信の効果が得られません。
適切にテキストを分割することで、生成と配信の並行処理が可能になります。";

while (string.IsNullOrWhiteSpace(apiKey = Console.ReadLine()))
{
    Console.WriteLine("api key is invalid");
}

while (string.IsNullOrWhiteSpace(modelUuid = Console.ReadLine()))
{
    Console.WriteLine("model uuid is invalid");
}

Aivis.AivisClientOptions options = new(apiKey!.Trim());
Aivis.AivisTTSClient ttsClient = new(options);
using var stream = await ttsClient.SynthesizeStreamAsync(modelUuid!.Replace("\n", "").Trim(), text!);

Aivis.Speakers.MP3Speaker speaker = new();
Console.WriteLine("チャンクの受信を開始します...");
Console.WriteLine("ストリーミング再生を開始します... ");
await speaker.PlayAsync(stream);
speaker.Dispose();