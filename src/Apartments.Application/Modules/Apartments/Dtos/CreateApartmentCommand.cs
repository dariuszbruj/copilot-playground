namespace Apartments.Application.Modules.Apartments.Dtos;

public record CreateApartmentCommand
{
    public required string Name { get; init; } = string.Empty;
    
    public required ApartmentAddressDto Address { get; init; }
}

public record ApartmentAddressDto
{
    public string Street { get; set; } = string.Empty;
    public string BuildingNo { get; set; } = string.Empty;
    public string FlatNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
