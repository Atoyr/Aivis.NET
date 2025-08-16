namespace Aivis;

public enum MediaType
{
    WAV,
    FLAC,
    MP3,
    AAC,
    OPUS,
}

public static class MediaTypeExtensions
{
    public static string ToFormatString(this MediaType mediaType)
    {
        return mediaType switch
        {
            MediaType.WAV => "wav",
            MediaType.FLAC => "flac",
            MediaType.MP3 => "mp3",
            MediaType.AAC => "aac",
            MediaType.OPUS => "opus",
            _ => throw new ArgumentOutOfRangeException(nameof(mediaType), mediaType, null)
        };
    }
}