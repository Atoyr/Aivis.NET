namespace Aivis;

public class AivisClient
{
    private AivisClientOptions _options;

    public AivisClient(AivisClientOptions options)
    {
        _options = options.Clone();
    }

}