namespace Apartments.Infrastructure.Apartments.Models;

public class ApartmentDbModel
{
    public int Id { get; set; }

    public Guid Guid { get; set; }

    public string Name { get; set; } = string.Empty;
}
