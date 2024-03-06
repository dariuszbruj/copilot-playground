using Apartments.Domain.Services.Apartments.Dtos;

namespace Apartments.Domain.Services.Apartments;

public interface IApartmentService
{
    Task<Result<Guid>> CreateApartment(CreateApartmentDto createApartmentDto, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<ApartmentDto>>> GetAsync(CancellationToken cancellationToken = default);

    Task<Result<ApartmentDto>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result> UpdateApartment(UpdateApartmentDto dto, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
