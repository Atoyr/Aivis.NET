using OpenTK.Audio.OpenAL;

namespace Aivis.Speakers;

/// <summary>
/// OpenALを使用して、デバイスの情報を取得するクラスです。
/// <summary>
/// <summary>
/// OpenALを使用して、デバイスの情報を取得するクラスです。
/// </summary>
public static class Device
{
    /// <summary>
    /// 再生デバイスの一覧を取得します。
    /// </summary>
    /// <returns>再生デバイスの名前の列挙。</returns>
    public static IEnumerable<string> ListPlaybackDevices()
    {
        // 互換（環境によって AllDevices が空のこともある）
        foreach (var name in ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier))
            yield return name;
    }

    /// <summary>
    /// デフォルトの再生デバイスを取得します。
    /// </summary>
    /// <return>デフォルトの再生デバイス名。</return>
    /// </summary>
    /// <returns>デフォルトの再生デバイス名。</returns>
    public static string DefaultPlaybackDevice()
    {
        // デフォルトの再生デバイス名を取得
        var name = ALC.GetString(ALC.GetContextsDevice(ALC.GetCurrentContext()), AlcGetString.DefaultDeviceSpecifier);
        return name ?? string.Empty;
    }
}