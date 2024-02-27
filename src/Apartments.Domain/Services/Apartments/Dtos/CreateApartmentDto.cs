namespace Apartments.Domain.Services.Apartments.Dtos;

public record CreateApartmentDto
{
    public string Name { get; init; } = string.Empty;
}
