namespace Apartments.Application.Modules.Tokens;

public interface ITokenGenerator
{
    string GenerateToken(string username);
}
