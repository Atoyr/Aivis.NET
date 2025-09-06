namespace Aivis;

public enum AudioChannel
{
    Mono,
    Stereo
}

public static class AudioChannelExtensions
{
    public static string ToFormatString(this AudioChannel channel)
    {
        return channel switch
        {
            AudioChannel.Mono => "mono",
            AudioChannel.Stereo => "stereo",
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }
}
