using System;
namespace Aivis;

public class AivisClient
{
    private readonly AivisClientOptions _options;

    public AivisClient(AivisClientOptions options)
    {
        _options = options.Clone();
    }

    private string GetApiUrl(string path) => _options.BaseUrl.TrimEnd('/') + path;
}