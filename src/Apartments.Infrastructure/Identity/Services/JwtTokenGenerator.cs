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
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            IssuedAt = timeProvider.GetUtcNow().DateTime,
            Expires = timeProvider.GetUtcNow().Add(_options.ExpirationTime).DateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            NotBefore = timeProvider.GetUtcNow().DateTime
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}
