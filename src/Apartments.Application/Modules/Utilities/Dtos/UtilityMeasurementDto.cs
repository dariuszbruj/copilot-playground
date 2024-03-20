namespace Apartments.Application.Modules.Utilities.Dtos;

public class UtilityMeasurementDto
{
    public Guid ApartmentId { get; set; }
    
    public DateTime MeasurementDate { get; set; }
    
    public decimal UtilityUsage { get; set; }
    
    public UtilityTypeDto UtilityType { get; set; }
}
