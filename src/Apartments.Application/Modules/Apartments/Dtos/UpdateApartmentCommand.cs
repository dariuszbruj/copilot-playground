namespace Apartments.Application.Modules.Apartments.Dtos;

public record UpdateApartmentCommand
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
