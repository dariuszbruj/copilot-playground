namespace Apartments.Domain.Services.AccountService;

public sealed record Password(string Value)
{
    public static implicit operator string(Password password) => password.Value;
}
