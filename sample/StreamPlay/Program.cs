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

// play streaming audio
using var memoryStream = new MemoryStream();

var buffer = new byte[8192];
int bytesRead = 0;
bool playbackStarted = false;
Aivis.Speaker speaker = new();

Console.WriteLine("チャンクの受信を開始します...");
while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
{
    // 受信したチャンクをメモリストリームに追加
    await memoryStream.WriteAsync(buffer, 0, bytesRead);

    // まだ再生が開始されていない場合、最初のチャンクで再生を試行
    if (!playbackStarted && memoryStream.Length > 4096) // 十分なデータが蓄積されたら
    {
        try
        {
            // メモリストリームの位置をリセット
            memoryStream.Position = 0;

            Console.WriteLine("ストリーミング再生を開始します... ");
            await speaker.PlayAsync(Aivis.MediaType.MP3, memoryStream);
            
            memoryStream.Position = memoryStream.Length;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"再生開始エラー（より多くのデータを待機中...）: {ex.Message}");
            // まだ十分なデータがない場合は続行
            memoryStream.Position = memoryStream.Length;
        }
    }
}

// まだ再生が開始されていない場合（小さなファイルなど）、ここで開始
if (!playbackStarted)
{
    try
    {
        Console.WriteLine("ストリーミング再生を開始します... ");
        await speaker.PlayAsync(Aivis.MediaType.MP3, memoryStream);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"再生エラー: {ex.Message}");
        return;
    }
}
