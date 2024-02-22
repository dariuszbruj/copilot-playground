using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apartments.Domain.Services;
using Apartments.Infrastructure.Identity.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            Expires = timeProvider.GetUtcNow().Add(_options.ExpirationTime).DateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
