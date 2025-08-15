using OpenTK.Audio.OpenAL;

namespace Aivis.Speakers;

public static class Device
{
    public static IEnumerable<string> ListPlaybackDevices()
    {
        // 互換（環境によって AllDevices が空のこともある）
        foreach (var name in ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier))
            yield return name;
    }

    public static string DefaultPlaybackDevice()
    {
        // デフォルトの再生デバイス名を取得
        var name = ALC.GetString(ALC.GetContextsDevice(ALC.GetCurrentContext()), AlcGetString.DefaultDeviceSpecifier);
        return name ?? string.Empty;
    }
}