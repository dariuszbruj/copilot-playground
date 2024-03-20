using Apartments.Application.Modules.Utilities.Dtos;

namespace Apartments.Application.Modules.Utilities;

public class UtilityService(IUtilityRepository apartmentRepository)
    : IUtilityService
{
    private readonly IUtilityRepository _apartmentRepository = apartmentRepository
        ?? throw new ArgumentNullException(nameof(apartmentRepository));
    
    public async Task InsertUtilityValues(UtilityMeasurementDto utilityMeasurementDto)
    {
        // check if apartment exists
        
        throw new NotImplementedException();
    }
}
