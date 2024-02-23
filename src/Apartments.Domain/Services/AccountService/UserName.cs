namespace Apartments.Domain.Services.AccountService;

public sealed record UserName(string Value)
{
    public static implicit operator string(UserName userName) => userName.Value;
}
