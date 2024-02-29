using Apartments.Domain.Services.Apartments.Results;

namespace Apartments.Domain.Services.Apartments;

public interface IApartmentRepository
{
    Task<Apartment> GetApartmentByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Apartment apartment, CancellationToken cancellationToken = default);
    Task<ApartmentResult>  UpdateAsync(Apartment apartment, CancellationToken cancellationToken = default);
    Task  DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
