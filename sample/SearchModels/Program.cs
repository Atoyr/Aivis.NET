Console.WriteLine("input mode");
Console.WriteLine(" 1: Search, 2: Detail, 3: Download");

var mode = Console.ReadLine();

switch (mode)
{
    case "1":
        await SearchModels();
        break;
    case "2":
        await GetModelDetail();
        break;
    case "3":
        await GetDownload();
        break;
    default:
        Console.WriteLine("Invalid mode selected.");
        break;
}


async Task SearchModels()
{
    var searchOptions = new Aivis.SearchVoiceModelsOptions();
    Console.WriteLine("input keyword");
    searchOptions.SetKeyword(Console.ReadLine());

    Console.WriteLine("input tags");
    searchOptions.SetKeyword(Console.ReadLine());

    Aivis.AivisClientOptions options = new();
    Aivis.AivisModelClient modelClient = new(options);
    var contents = await modelClient.SearchModels(searchOptions);

    Console.WriteLine($"Found {contents.Total} models:");

    foreach (var model in contents.AivmModels)
    {
        Console.WriteLine($"- {model.Name} ({model.AivmModelUuid})");
        Console.WriteLine($"  Description: {model.Description}");
        Console.WriteLine($"  Tags: {string.Join(", ", model.Tags.Select(x => x.Name) ?? new Aivis.Schemas.Tag[0].Select(x => x.Name))}");
        Console.WriteLine($"  Created At: {model.CreatedAt}");
        Console.WriteLine($"  Updated At: {model.UpdatedAt}");
        Console.WriteLine();
    }
}

async Task GetModelDetail()
{
    Console.WriteLine("input Model ID");
    var id = Console.ReadLine();
    Aivis.AivisClientOptions options = new();
    Aivis.AivisModelClient modelClient = new(options);
    var model = await modelClient.GetModelDetail(id);

    Console.WriteLine($"- {model.Name} ({model.AivmModelUuid})");
    Console.WriteLine($"  Description: {model.Description}");
    Console.WriteLine($"  Tags: {string.Join(", ", model.Tags.Select(x => x.Name) ?? new Aivis.Schemas.Tag[0].Select(x => x.Name))}");
    Console.WriteLine($"  Created At: {model.CreatedAt}");
    Console.WriteLine($"  Updated At: {model.UpdatedAt}");
    Console.WriteLine();
}

async Task GetDownload()
{
    Console.WriteLine("input Model ID");
    var id = Console.ReadLine();
    Aivis.AivisClientOptions options = new();
    Aivis.AivisModelClient modelClient = new(options);
    var url = await modelClient.GetDownloadUrl(id);

    Console.WriteLine($"Download URL: {url}");
    Console.WriteLine();
}