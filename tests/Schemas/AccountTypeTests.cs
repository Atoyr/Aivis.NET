using System.Text.Json;

using Xunit;

namespace Aivis.Schemas.Tests;

public class AccountTypeTests
{
    [Theory]
    [InlineData("User", AccountType.User)]
    [InlineData("Official", AccountType.Official)]
    [InlineData("Admin", AccountType.Admin)]
    public void Should_DeserializeFromString_Success(string jsonValue, AccountType expected)
    {
        // Arrange
        string json = $"\"{jsonValue}\"";

        // Act
        var result = JsonSerializer.Deserialize<AccountType>(json);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(AccountType.User, "User")]
    [InlineData(AccountType.Official, "Official")]
    [InlineData(AccountType.Admin, "Admin")]
    public void Should_SerializeToString_Success(AccountType value, string expected)
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
        string json = "\"InvalidType\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<AccountType>(json));
    }
}