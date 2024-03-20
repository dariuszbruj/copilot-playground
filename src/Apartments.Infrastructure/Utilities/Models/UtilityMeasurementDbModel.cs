using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apartments.Infrastructure.Utilities.Models;

public class UtilityMeasurementDbModel
{
    public Guid ApartmentId { get; set; }
    public DateTime MeasurementDate { get; set; }
    public int UtilityType { get; set; }
    public decimal UtilityUsage { get; set; }
}

public class UtilityMeasurementDbModelConfiguration : IEntityTypeConfiguration<UtilityMeasurementDbModel>
{
    public void Configure(EntityTypeBuilder<UtilityMeasurementDbModel> builder)
    {
        builder.HasKey(um => new { um.ApartmentId, um.MeasurementDate, um.UtilityType });
        builder.ToTable("UtilityMeasurements");

        builder.Property(um => um.MeasurementDate)
            .IsRequired();

        builder.Property(um => um.UtilityType)
            .IsRequired();

        builder.Property(um => um.UtilityUsage)
            .IsRequired();
    }
}
