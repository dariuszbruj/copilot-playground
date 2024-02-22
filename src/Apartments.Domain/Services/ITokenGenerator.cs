namespace Apartments.Domain.Services;

public interface ITokenGenerator
{
    string GenerateToken(string username);
}
