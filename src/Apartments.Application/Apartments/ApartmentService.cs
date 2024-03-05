using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Dtos;
using Apartments.WebApi.Requests;

namespace Apartments.Application.Apartments;

public class ApartmentService(IApartmentRepository apartmentRepository)
{
    private readonly IApartmentRepository _apartmentRepository = apartmentRepository
        ?? throw new ArgumentNullException(nameof(apartmentRepository));

    public async Task<Result<Guid>> CreateApartment(CreateApartmentDto createApartmentDto,
        CancellationToken cancellationToken  = default)
    {
        var apartment = new Apartment
        {
            Name = createApartmentDto.Name,
            Address = new Address()
            {
                Street = createApartmentDto.Address.Street,
                City = createApartmentDto.Address.City,
                State = createApartmentDto.Address.State,
                ZipCode = createApartmentDto.Address.ZipCode,
                BuildingNo = createApartmentDto.Address.BuildingNo,
                FlatNumber = createApartmentDto.Address.FlatNumber
            }
        };
        
        var guid = await _apartmentRepository.AddAsync(apartment, cancellationToken);
        
        return Result<Guid>.Ok(guid);
    }

    public async Task<Result<IEnumerable<Apartment>>> GetAsync(
        CancellationToken cancellationToken = default)
    {
        var apartments = await _apartmentRepository
            .GetApartmentsAsync( cancellationToken);
        
        return Result<IEnumerable<Apartment>>.Ok(apartments);
    }
    
    public async Task<Result<Apartment>> GetAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        var apartment = await _apartmentRepository
            .GetApartmentByIdAsync(id, cancellationToken);
        
        return Result<Apartment>.Ok(apartment);
    }

    public async Task<Result> UpdateApartment(UpdateApartmentDto dto, 
        CancellationToken cancellationToken = default)
    {
        var apartment = await _apartmentRepository
            .GetApartmentByIdAsync(dto.Id, cancellationToken);
        
        if (dto.Name is not null)
        {
            apartment.Name = dto.Name;
        }
        
        await _apartmentRepository.UpdateAsync(apartment, cancellationToken);

        return Result.Ok();
    }

    public async Task DeleteAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        await _apartmentRepository.DeleteAsync(id, cancellationToken);
    }
}
