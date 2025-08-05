using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Aivis.Schemas.Tests;

public class AivmModelResponseTests
{
    private readonly ITestOutputHelper _output;

    public AivmModelResponseTests(ITestOutputHelper output)
    {
        _output = output;
    }

//     public string Json = @""" {""aivm_model_uuid"":""a59cb814-0083-4369-8542-f51a29e72af7"",""user"":{""handle"":""aivis_project"",""name"":""Aivis Project"",""description"":""Aivis Project の公式アカウントです。\n音声合成モデルの公式配布を行なっています。"",""icon_url"":""https://assets.aivis-project.com/account-icons/3889b76f-2c8b-41aa-a359-fa6603fdbf60.jpg"",""account_type"":""Admin"",""account_status"":""Active"",""social_links"":[{""type"":""Twitter"",""url"":""https://x.com/aivis_project""},{""type"":""GitHub"",""url"":""https://github.com/Aivis-Project""},{""type"":""Website"",""url"":""https://note.com/jpchain/""},{""type"":""Website"",""url"":""https://aivis-project.com/""}]},""name"":""Anneli"",""description"":""AivisSpeech に標準搭載されている音声合成モデルです。"",""detailed_description"":"""",""category"":""OriginalCharacter"",""voice_timbre"":""YouthfulFemale"",""visibility"":""Public"",""is_tag_locked"":false,""total_download_count"":65467,""model_files"":[{""aivm_model_uuid"":""a59cb814-0083-4369-8542-f51a29e72af7"",""manifest_version"":""1.0"",""name"":""Anneli"",""description"":""AivisSpeech に標準搭載されている音声合成モデルです。"",""creators"":[""Aivis Project""],""license_type"":""ACML 1.0"",""license_text"":null,""model_type"":""AIVM"",""model_architecture"":""Style-Bert-VITS2 (JP-Extra)"",""model_format"":""Safetensors"",""training_epochs"":116,""training_steps"":32000,""version"":""1.0.1"",""file_size"":257639874,""checksum"":""51a69c4218b73218a5066535eca9545fcc4154409cd157fd357bf42bed409ae6"",""download_count"":186,""created_at"":""2025-04-06T07:59:47.064137+09:00"",""updated_at"":""2025-08-01T19:10:51.099067+09:00""} """;
// 
//     [Fact]
//     public void Should_DeserializeEnumWithSpaceValue_Success()
//     {
//         // Act & Assert
//         var result = JsonSerializer.Deserialize<Aivis.Schemas.AivmModelResponse>(Json);
//         
//         Assert.NotNull(result);
//         Assert.Single(result.ModelFiles);
//         Assert.Equal(LicenseType.ACML_1_0, result.ModelFiles[0].LicenseType);
//         Assert.Equal("Aivis Project", result.ModelFiles[0].Name);
//         
//         _output.WriteLine($"デシリアライゼーション成功: {result.ModelFiles[0].LicenseType}");
//     }

    [Fact]
    public void Should_DeserializeDirectEnumValue_Success()
    {
        // Arrange - まず enum だけをテスト
        const string json = @"""ACML 1.0""";

        // Act
        var result = JsonSerializer.Deserialize<Aivis.Schemas.LicenseType>(json);

        // Assert
        Assert.Equal(LicenseType.ACML_1_0, result);
        _output.WriteLine($"直接enum変換成功: {result}");
    }
}