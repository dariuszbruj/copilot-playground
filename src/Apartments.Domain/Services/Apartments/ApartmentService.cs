using Apartments.Api.UnitTests.Domain;

namespace Apartments.Domain.Services.Apartments;

public class ApartmentService
{
    public async Task<Apartment> CreateApartment(Apartment apartment)
    {
        // Your implementation goes here
        return apartment;
    }

    public async Task<Apartment> GetApartment(Guid id)
    {
        // Your implementation goes here
        return new Apartment();
    }

    public async Task<Apartment> UpdateApartment(Apartment apartment)
    {
        // Your implementation goes here
        return apartment;
    }

    public async Task<bool> DeleteApartment(Guid id)
    {
        // Your implementation goes here
        return true;
    }
}
