Console.WriteLine("input mode");
Console.WriteLine(" 1: GetMe, 2: GetUser");

var mode = Console.ReadLine();

switch (mode)
{
    case "1":
        await SearchModels();
        break;
    case "2":
        throw new NotImplementedException("GetUser is not implemented in this sample.");
        break;
    default:
        Console.WriteLine("Invalid mode selected.");
        break;
}


async Task SearchModels()
{
    string? apiKey = null;
    var searchOptions = new Aivis.SearchVoiceModelsOptions();
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