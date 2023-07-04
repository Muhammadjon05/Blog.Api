using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blog.Api.Entities;
using Blog.Api.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Api.Managers;

public class TokenManager
{
    private readonly JwtOptions _options;

    public TokenManager(IOptions<JwtOptions>? options)
    {
        _options = options!.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };
        var signingKey = System.Text.Encoding.UTF32.GetBytes(_options.SigningKey);
        var security = new JwtSecurityToken(
            issuer: _options.ValidIssuer, audience: _options.ValidAudience, claims: claims,
            expires: DateTime.Now.AddMinutes(_options.ExpiresInMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256)
        );
        var token = new JwtSecurityTokenHandler().WriteToken(security);
        return token;
    }
}