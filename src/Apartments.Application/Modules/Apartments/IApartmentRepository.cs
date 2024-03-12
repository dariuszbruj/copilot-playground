using Apartments.Application.Modules.Apartments.Results;
using Apartments.Domain.Models;

namespace Apartments.Application.Modules.Apartments;

public interface IApartmentRepository
{
    Task<Apartment> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> AddAsync(Apartment apartment, CancellationToken cancellationToken = default);
    Task<ApartmentResult>  UpdateAsync(Apartment apartment, CancellationToken cancellationToken = default);
    Task  DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Apartment>> GetApartmentsAsync(CancellationToken cancellationToken = default);
}
