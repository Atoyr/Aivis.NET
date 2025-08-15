Console.WriteLine("input mode");
Console.WriteLine(" 1: GetMe, 2: GetUserInfo");

var mode = Console.ReadLine();

switch (mode)
{
    case "1":
        await GetMe();
        break;
    case "2":
        await GetUserInfo();
        break;
    default:
        Console.WriteLine("Invalid mode selected.");
        break;
}


async Task GetMe()
{
    string? apiKey = null;
    var searchOptions = new Aivis.SearchVoiceModelsOptions();
    while(string.IsNullOrWhiteSpace(apiKey))
{
    string? apiKey = null;
    while(string.IsNullOrWhiteSpace(apiKey))
    {
        Console.WriteLine("input api key");
        apiKey = Console.ReadLine();
    }

    Aivis.AivisClientOptions options = new(apiKey);
    Aivis.AivisUsersClient usersClient = new(options);
    var contents = await usersClient.GetMe();

    Console.WriteLine("User Information:");
    Console.WriteLine($"HandleName: {contents.Handle}");
    Console.WriteLine($"Name: {contents.Name}");
    Console.WriteLine($"Email: {contents.Email}");

    Console.WriteLine($"CreditBalance: {contents.CreditBalance}");
}

async Task GetUserInfo()
{
    string? handle = null;
    var searchOptions = new Aivis.SearchVoiceModelsOptions();
    while(string.IsNullOrWhiteSpace(handle))
    {
        Console.WriteLine("input handle");
        handle = Console.ReadLine();
    }

    Aivis.AivisClientOptions options = new();
    Aivis.AivisUsersClient usersClient = new(options);
    var contents = await usersClient.GetUserInfo(handle);

    Console.WriteLine("User Information:");
    Console.WriteLine($"HandleName: {contents.Handle}");
    Console.WriteLine($"Name: {contents.Name}");
}