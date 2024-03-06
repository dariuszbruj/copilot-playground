namespace Apartments.Domain.Models;

public record Apartment
{
    public Guid Id { get; set; }
    public Address Address { get; set; } = new();
    public string Name { get; set; } = string.Empty;
}
