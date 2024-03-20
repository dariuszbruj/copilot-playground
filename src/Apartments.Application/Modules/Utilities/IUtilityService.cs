using Apartments.Application.Modules.Utilities.Dtos;

namespace Apartments.Application.Modules.Utilities;

public interface IUtilityService
{
    Task InsertUtilityValues(UtilityMeasurementDto utilityMeasurementDto);
}
