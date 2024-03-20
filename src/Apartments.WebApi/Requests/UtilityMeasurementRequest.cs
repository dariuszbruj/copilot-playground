namespace Apartments.WebApi.Requests;

/// <summary>
/// Represents a request for a utility measurement.
/// </summary>
public record UtilityMeasurementRequest
{
    /// <summary>
    /// Gets or sets the date of the measurement.
    /// </summary>
    public DateTime MeasurementDate { get; init; }

    /// <summary>
    /// Gets or sets the usage of the utility.
    /// </summary>
    public decimal UtilityUsage { get; init; }

    /// <summary>
    /// Gets or sets the type of the utility.
    /// </summary>
    public UtilityTypeRequest UtilityType { get; init; }
}

/// <summary>
/// Represents the type of a utility.
/// </summary>
public enum UtilityTypeRequest
{
    /// <summary>
    /// Represents water as a utility type.
    /// </summary>
    Water,

    /// <summary>
    /// Represents electricity as a utility type.
    /// </summary>
    Electricity,

    /// <summary>
    /// Represents gas as a utility type.
    /// </summary>
    Gas
}
