using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apartments.Application.Modules.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Apartments.Infrastructure.Identity.Services;

public class JwtTokenGenerator(TimeProvider timeProvider, IOptions<JwtTokenGeneratorOptions> options)
    : ITokenGenerator
{
    private readonly JwtTokenGeneratorOptions _options = options.Value;

    public string GenerateToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.Key);
        var now = timeProvider.GetUtcNow();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            Expires = now.Add(_options.ExpirationTime).UtcDateTime,
            NotBefore = now.UtcDateTime,
            IssuedAt = now.UtcDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
