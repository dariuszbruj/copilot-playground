namespace Apartments.Domain.Services.Apartments.Dtos;

public record CreateApartmentDto
{
    public required string Name { get; init; } = string.Empty;
    
    public required CreateApartmentAddressDto Address { get; init; }
}

public record CreateApartmentAddressDto
{
    public string Street { get; set; } = string.Empty;
    public string BuildingNo { get; set; } = string.Empty;
    public string FlatNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
