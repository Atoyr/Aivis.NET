using System.Text.Json;

using Xunit;

namespace Aivis.Schemas.Tests;

public class AccountStatusTests
{
    [Theory]
    [InlineData("Active", AccountStatus.Active)]
    [InlineData("Suspended", AccountStatus.Suspended)]
    public void Should_DeserializeFromString_Success(string jsonValue, AccountStatus expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<AccountStatus>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(AccountStatus.Active, "Active")]
    [InlineData(AccountStatus.Suspended, "Suspended")]
    public void Should_SerializeToString_Success(AccountStatus value, string expected)
    {
        // Act
        var result = JsonSerializer.Serialize(value);

        // Assert
        Assert.Equal($"\"{expected}\"", result);
    }

    [Fact]
    public void Should_ThrowException_WhenInvalidValue()
    {
        // Arrange
        string json = "\"InvalidStatus\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<AccountStatus>(json));
    }
}