using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apartments.Infrastructure.Apartments.Models;

public class ApartmentAddressDbModel
{
    public int Id { get; init; }
    
    public string Street { get; init; } = string.Empty;

    public string BuildingNumber { get; init; } = string.Empty;

    public string FlatNumber { get; init; } = string.Empty;

    public string ZipCode { get; init; } = string.Empty;
    
    public string City { get; init; } = string.Empty;

    public string State { get; init; } = string.Empty;
}

public class ApartmentAddressDbModelConfiguration : IEntityTypeConfiguration<ApartmentAddressDbModel>
{
    public void Configure(EntityTypeBuilder<ApartmentAddressDbModel> builder)
    {
        builder.HasKey(a => a.Id);
        builder.ToTable("ApartmentAddresses");
        
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(x => x.Street)
            .HasMaxLength(255);

        builder.Property(x => x.BuildingNumber)
            .HasMaxLength(50);

        builder.Property(x => x.FlatNumber)
            .HasMaxLength(50);

        builder.Property(x => x.ZipCode)
            .HasMaxLength(50);

        builder.Property(x => x.City)
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(50);
    }
}
