namespace Apartments.Domain.Models;

public class UtilityMeasurement
{
    public Guid ApartmentId { get; set; }
    public DateTime MeasurementDate { get; set; }
    public decimal UtilityUsage { get; set; }
    public UtilityType UtilityType { get; set; }
}
