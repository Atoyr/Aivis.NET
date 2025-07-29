namespace Aivis;

public class AivisClientOptions
{

    internal string ApiKey { init; get; }

    public string BaseUrl { init; get; } = "https://api.aivis-project.com";

    public AivisClientOptions(string apiKey)
    {
        ApiKey = apiKey;
    }

    public AivisClientOptions Clone()
    {
        return new AivisClientOptions(ApiKey)
        {
            BaseUrl = BaseUrl
        };
    }
}