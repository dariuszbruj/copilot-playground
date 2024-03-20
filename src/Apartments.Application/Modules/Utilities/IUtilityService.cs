using Apartments.Domain.Models;

namespace Apartments.Application.Modules.Utilities;

public interface IUtilityService
{
    Task InsertUtilityValues(UtilityMeasurement utilityMeasurement);
}
