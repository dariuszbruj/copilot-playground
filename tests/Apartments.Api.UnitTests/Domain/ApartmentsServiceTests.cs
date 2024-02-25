using Apartments.Domain.Services.Apartments;

namespace Apartment.Api.UnitTests.Domain;

public class ApartmentServiceTests
{
    private readonly ApartmentService _apartmentService = new();

    [Fact]
    public async Task CanCreateApartment()
    {
        var apartment = new Apartments.Api.UnitTests.Domain.Apartment { /* set properties here */ };
        var result = await _apartmentService.CreateApartment(apartment);
        Assert.NotNull(result);
        // Add more assertions based on your business rules
    }

    [Fact]
    public async Task CanReadApartment()
    {
        var id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc");
        var result = await _apartmentService.GetApartment(id);
        Assert.NotNull(result);
        // Add more assertions based on your business rules
    }

    [Fact]
    public async Task CanUpdateApartment()
    {
        var apartment = new Apartments.Api.UnitTests.Domain.Apartment { /* set properties here */ };
        var result = await _apartmentService.UpdateApartment(apartment);
        Assert.NotNull(result);
        // Add more assertions based on your business rules
    }

    [Fact]
    public async Task CanDeleteApartment()
    {
        var id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc");
        var result = await _apartmentService.DeleteApartment(id);
        Assert.True(result);
    }
}
