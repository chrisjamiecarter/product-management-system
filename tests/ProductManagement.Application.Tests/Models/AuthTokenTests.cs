using ProductManagement.Application.Models;

namespace ProductManagement.Application.Tests.Models;

/// <summary>
/// Unit tests for the <see cref="AuthToken"/> class, ensuring correct encoding and decoding behavior.
/// </summary>
public class AuthTokenTests
{
    [Theory]
    [InlineData("simple-token")]
    [InlineData("token@with.special!characters")]
    [InlineData("token with spaces")]
    [InlineData("1234567890")]
    [InlineData("längërTōkęnWîthÛnicødę")]
    public void Encode_ThenDecode_ShouldReturnOriginalValue(string token)
    {
        // Arrange.
        var encoded = AuthToken.Encode(token);

        // Act.
        var result = AuthToken.Decode(encoded.Code);

        // Assert.
        Assert.Equal(token, result.Value);
        Assert.Equal(encoded.Value, result.Value);
        Assert.Equal(encoded.Code, result.Code);
    }

    [Theory]
    [InlineData("simple-token")]
    [InlineData("token@with.+special!/characters==")]
    public void Encode_ShouldCreateValidBase64UrlString(string token)
    {
        // Base64URL:
        // - Replaces + with -
        // - Replaces / with _
        // - Removes padding

        // Arrange.

        // Act.
        var result = AuthToken.Encode(token);

        // Assert.
        Assert.DoesNotContain("+", result.Code);
        Assert.DoesNotContain("/", result.Code);
        Assert.DoesNotContain("=", result.Code);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Encode_ShouldEncodeAndDecode_WhenTokenIsEmptyOrWhitespace(string token)
    {
        // Arrange.
        var encoded = AuthToken.Encode(token);

        // Act.
        var result = AuthToken.Decode(encoded.Code);

        // Assert.
        Assert.Equal(token, result.Value);
        Assert.Equal(encoded.Value, result.Value);
        Assert.Equal(encoded.Code, result.Code);
    }

    [Fact]
    public void Encode_ShouldEncodeAndDecode_WhenTokenIsLong()
    {
        // Arrange.
        var token = new string('a', 1000);
        var encoded = AuthToken.Encode(token);

        // Act
        var result = AuthToken.Decode(encoded.Code);

        // Assert
        Assert.Equal(token, encoded.Value);
        Assert.Equal(encoded.Value, result.Value);
        Assert.Equal(encoded.Code, result.Code);
    }

    [Theory]
    [InlineData(null)]
    public void Encode_ShouldThrowArgumentNullException_WhenTokenIsNull(string? token)
    {
        // Arrange.

        // Act.

        // Assert.
        Assert.Throws<ArgumentNullException>(() => AuthToken.Encode(token!));
    }

    [Theory]
    [InlineData(null)]
    public void Decode_ShouldThrowArgumentNullException_WhenCodeIsNull(string? code)
    {
        // Arrange.

        // Act.

        // Assert.
        Assert.Throws<ArgumentNullException>(() => AuthToken.Decode(code!));
    }
}
