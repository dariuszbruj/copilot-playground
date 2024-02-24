namespace Apartments.Domain.Services.AccountService.Models;

public sealed record Password(string Value)
{
    public static implicit operator string(Password password) => password.Value;
}
