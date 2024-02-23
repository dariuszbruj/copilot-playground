namespace Apartments.Domain.Services.AccountService;

public record LockoutOnFailure(bool Enabled)
{
    public static implicit operator bool(LockoutOnFailure lockoutOnFailure) => lockoutOnFailure.Enabled;
}
