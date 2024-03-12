namespace Apartments.Application.Modules.AccountServices.Models;

public readonly record struct Password(string Value)
{
    public string Value { get; } =
        string.IsNullOrEmpty(Value)
            ? throw new ArgumentException("Invalid argument", nameof(Value))
            : Value;

    public static implicit operator string(Password @object) =>
        @object.Value;

    public static explicit operator Password(string value) =>
        new(value);
}
