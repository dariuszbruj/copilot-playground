using Apartments.Domain;
using Apartments.Domain.Services.Apartments;

namespace Apartments.Application.Apartments;

public class ApartmentService(IApartmentRepository apartmentRepository)
{
    private readonly IApartmentRepository _apartmentRepository = apartmentRepository 
        ?? throw new ArgumentNullException(nameof(apartmentRepository));

    public async Task CreateApartment(Apartment apartment)
    {
        // Your implementation goes here
        await _apartmentRepository.AddAsync(apartment);
    }

    public async Task<Apartment> GetApartment(Guid id)
    {
        // Your implementation goes here
        var result =  await _apartmentRepository.GetApartmentByIdAsync(id);

        // TODO: finish it.
        return null;
    }

    public async Task UpdateApartment(Apartment apartment)
    {
        // Your implementation goes here
        await _apartmentRepository.UpdateAsync(apartment);
    }

    public async Task DeleteApartment(Guid id)
    {
        // Your implementation goes here
        await _apartmentRepository.DeleteAsync(id);
    }
}
