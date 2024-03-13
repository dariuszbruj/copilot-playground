using Apartments.Application.Common;
using Apartments.Application.Modules.Apartments.Dtos;
using Apartments.Domain.Models;

namespace Apartments.Application.Modules.Apartments;

public class ApartmentService(IApartmentRepository apartmentRepository)
    : IApartmentService
{
    private readonly IApartmentRepository _apartmentRepository = apartmentRepository
        ?? throw new ArgumentNullException(nameof(apartmentRepository));

    public async Task<Result<Guid>> CreateAsync(CreateApartmentCommand createApartmentCommand,
        CancellationToken cancellationToken  = default)
    {
        var apartment = new Apartment
        {
            Name = createApartmentCommand.Name,
            Address = new Address
            {
                Street = createApartmentCommand.Address.Street,
                City = createApartmentCommand.Address.City,
                State = createApartmentCommand.Address.State,
                ZipCode = createApartmentCommand.Address.ZipCode,
                BuildingNo = createApartmentCommand.Address.BuildingNo,
                FlatNumber = createApartmentCommand.Address.FlatNumber
            }
        };
        
        var guid = await _apartmentRepository.AddAsync(apartment, cancellationToken);
        
        return Result<Guid>.Ok(guid);
    }

    public async Task<Result<IEnumerable<ApartmentDto>>> GetAsync(
        CancellationToken cancellationToken = default)
    {
        var apartments = await _apartmentRepository
            .GetApartmentsAsync( cancellationToken);
        
        return Result<IEnumerable<ApartmentDto>>
            .Ok(apartments.Select(apartment => new ApartmentDto { 
                Id = apartment.Id, 
                Name = apartment.Name, 
                Address = new ApartmentAddressDto
                {
                    Street = apartment.Address.Street,
                    City = apartment.Address.City,
                    State = apartment.Address.State,
                    ZipCode = apartment.Address.ZipCode,
                    BuildingNo = apartment.Address.BuildingNo,
                    FlatNumber = apartment.Address.FlatNumber
                } 
            }));
    }
    
    public async Task<Result<ApartmentDto>> GetAsync(Guid id, 
        CancellationToken cancellationToken = default)
    {
        var apartment = await _apartmentRepository
            .GetByIdAsync(id, cancellationToken);
        
        return Result<ApartmentDto>.Ok(
            new ApartmentDto
            {
                Id = apartment.Id, 
                Name = apartment.Name,
                Address = new ApartmentAddressDto
                {
                    Street = apartment.Address.Street,
                    City = apartment.Address.City,
                    State = apartment.Address.State,
                    ZipCode = apartment.Address.ZipCode,
                    BuildingNo = apartment.Address.BuildingNo,
                    FlatNumber = apartment.Address.FlatNumber
                }
            });
    }

    public async Task<Result> UpdateApartment(UpdateApartmentCommand command, 
        CancellationToken cancellationToken = default)
    {
        var apartment = await _apartmentRepository
            .GetByIdAsync(command.Id, cancellationToken);
        
        if (command.Name is not null)
        {
            apartment.Name = command.Name;
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
