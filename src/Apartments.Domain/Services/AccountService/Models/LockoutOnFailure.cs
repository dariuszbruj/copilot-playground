namespace Apartments.Domain.Services.AccountService.Models;

public record LockoutOnFailure(bool Enabled)
{
    public static implicit operator bool(LockoutOnFailure lockoutOnFailure) => lockoutOnFailure.Enabled;
}
