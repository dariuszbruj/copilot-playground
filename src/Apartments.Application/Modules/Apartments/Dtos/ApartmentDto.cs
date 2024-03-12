namespace Apartments.Application.Modules.Apartments.Dtos;

public record struct ApartmentDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
