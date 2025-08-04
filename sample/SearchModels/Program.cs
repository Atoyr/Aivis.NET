var searchOptions = new Aivis.SearchVoiceModelsOptions();
Console.WriteLine("input keyword");
searchOptions.SetKeyword(Console.ReadLine());

Console.WriteLine("input tags");
searchOptions.SetKeyword(Console.ReadLine());

Aivis.AivisClientOptions options = new();
Aivis.AivisModelClient modelClient = new(options);
var contents = await modelClient.SearchModels(searchOptions);

Console.WriteLine($"Found {contents.Total} models:");

foreach(var model in contents.AivmModels)
{
    Console.WriteLine($"- {model.Name} ({model.AivmModelUuid})");
    Console.WriteLine($"  Description: {model.Description}");
    Console.WriteLine($"  Tags: {string.Join(", ", model.Tags ?? new string[0])}");
    Console.WriteLine($"  Created At: {model.CreatedAt}");
    Console.WriteLine($"  Updated At: {model.UpdatedAt}");
    Console.WriteLine();
}