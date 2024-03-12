using Apartments.Application.Common;
using Apartments.Application.Modules.Apartments.Dtos;

namespace Apartments.Application.Modules.Apartments;

public interface IApartmentService
{
    Task<Result<Guid>> CreateAsync(CreateApartmentCommand createApartmentCommand, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<ApartmentDto>>> GetAsync(CancellationToken cancellationToken = default);

    Task<Result<ApartmentDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> UpdateApartment(UpdateApartmentCommand command, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
