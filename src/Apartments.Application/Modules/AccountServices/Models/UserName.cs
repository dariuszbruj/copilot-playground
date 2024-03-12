namespace Apartments.Application.Modules.AccountServices.Models;

public readonly record struct UserName(string Value)
{
    public string Value { get; } =
        string.IsNullOrEmpty(Value)
            ? throw new ArgumentException("Invalid argument", nameof(Value))
            : Value;

    public static implicit operator string(UserName @object) =>
        @object.Value;

    public static explicit operator UserName(string value) =>
        new(value);
}
