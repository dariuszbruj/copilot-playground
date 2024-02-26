using Apartments.Domain.Services.Apartments.Results;

namespace Apartments.Domain.Services.Apartments;

public interface IApartmentRepository
{
    Task<ApartmentResult> GetApartmentByIdAsync(Guid id);
    Task AddAsync(Apartment apartment);
    Task<ApartmentResult>  UpdateAsync(Apartment apartment);
    Task  DeleteAsync(Guid id);
}
