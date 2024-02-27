namespace Apartments.Domain.Services.Apartments.Dtos;

public record UpdateApartmentDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
