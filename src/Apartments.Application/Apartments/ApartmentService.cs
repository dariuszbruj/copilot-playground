using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Dtos;
using Apartments.WebApi.Requests;

namespace Apartments.Application.Apartments;

public class ApartmentService(IApartmentRepository apartmentRepository)
{
    public async Task<Result<Apartment>> CreateApartment(CreateApartmentDto createApartmentDto,
        CancellationToken cancellationToken  = default)
    {
        var apartment = new Apartment
        {
            Name = createApartmentDto.Name
        };
        
        await apartmentRepository.AddAsync(apartment, cancellationToken);
        
        return Result<Apartment>.Ok(apartment);
    }

    public async Task<Result<Apartment>> GetAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        var apartment = await apartmentRepository
            .GetApartmentByIdAsync(id, cancellationToken);
        
        return Result<Apartment>.Ok(apartment);
    }

    public async Task<Result> UpdateApartment(UpdateApartmentDto dto, 
        CancellationToken cancellationToken = default)
    {
        // TODO: UnitOfWork
        var apartment = await apartmentRepository
            .GetApartmentByIdAsync(dto.Id, cancellationToken);
        
        // TODO: Update apartment
        
        await apartmentRepository.UpdateAsync(apartment, cancellationToken);

        return Result.Ok();
    }

    public async Task DeleteAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        await apartmentRepository.DeleteAsync(id, cancellationToken);
    }
}
