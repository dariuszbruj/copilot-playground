namespace Apartments.WebApi.Requests;

/// <summary>
/// 
/// </summary>
public class UtilityMeasurementRequest
{
    /// <summary>
    /// 
    /// </summary>
    public Guid ApartmentId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime MeasurementDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public decimal UtilityUsage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public UtilityTypeRequest UtilityType { get; set; }
}

/// <summary>
/// 
/// </summary>
public enum UtilityTypeRequest
{
    /// <summary>
    /// 
    /// </summary>
    Water,

    /// <summary>
    /// 
    /// </summary>
    Electricity,

    /// <summary>
    /// 
    /// </summary>
    Gas
}
