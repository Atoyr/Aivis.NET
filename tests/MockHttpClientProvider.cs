namespace Aivis.Tests;

public class MockHttpClientProvider : IHttpClientProvider
{
    public MockHttpClientProvider(HttpClient httpClient)
    {
        Instance = httpClient;
    }

    public HttpClient Instance { get; private set; }
}