namespace Apartments.Domain;

public record Apartment
{
    public Guid Id { get; set; }
    public Address Address { get; set; } = new();
    public string Name { get; set; } = string.Empty;
}

public record Address
{
    public string Street { get; set; } = string.Empty;
    public string BuildingNo { get; set; }= string.Empty;
    public string FlatNumber { get; set; }= string.Empty;
    public string City { get; set; }= string.Empty;
    public string State { get; set; }= string.Empty;
    public string ZipCode { get; set; }= string.Empty;
}
