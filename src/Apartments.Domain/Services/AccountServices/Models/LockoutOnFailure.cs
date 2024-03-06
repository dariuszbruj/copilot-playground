namespace Apartments.Domain.Services.AccountServices.Models;

public record struct LockoutOnFailure(bool Enabled)
{
    public static implicit operator bool(LockoutOnFailure @object) =>
        @object.Enabled;
    
    public static explicit operator LockoutOnFailure(bool value) =>
        new(value);
}
