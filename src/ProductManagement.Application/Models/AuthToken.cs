using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace ProductManagement.Application.Models;

/// <summary>
/// Represents a token that can be encoded to and decoded from a Base64 URL encoded string.
/// The Code is the encoded token value.
/// The Value is the original token value.
/// </summary>
public sealed class AuthToken
{
    private AuthToken(string code, string value)
    {
        Code = code;
        Value = value;
    }

    public string Value { get; }

    public string Code { get; }

    public static AuthToken Encode(string token)
    {
        var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        return new AuthToken(code, token);
    }

    public static AuthToken Decode(string code)
    {
        var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        return new AuthToken(code, token);
    }
}
