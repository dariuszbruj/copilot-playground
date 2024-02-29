using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apartments.Infrastructure.Apartments.Models;

public class ApartmentDbModel
{
    public int Id { get; init; }

    public Guid Guid { get; init; }

    public string Name { get; set; } = string.Empty;

    public ApartmentAddressDbModel? Address { get; set; }
}


public class ApartmentDbModelConfiguration : IEntityTypeConfiguration<ApartmentDbModel>
{
    public void Configure(EntityTypeBuilder<ApartmentDbModel> builder)
    {
        builder.HasKey(a => a.Id);
        builder.ToTable("Apartments");

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Guid)
            .HasDefaultValueSql("newid()");

        builder.Property(a => a.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}
