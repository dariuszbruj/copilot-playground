using Apartments.Application.Apartments;
using Apartments.Domain.Services.Apartments;
using FakeItEasy;

namespace Apartment.Api.UnitTests.Domain;

public class ApartmentServiceTests
{
    private readonly ApartmentService _apartmentService;
    private readonly IApartmentRepository _apartmentRepository;

    public ApartmentServiceTests()
    {
        _apartmentRepository = A.Fake<IApartmentRepository>();
        _apartmentService = new ApartmentService(_apartmentRepository);
    }

    [Fact]
    public async Task CanCreateApartment()
    {
        var apartment = new Apartments.Domain.Apartment();
        //var result = await _apartmentService.CreateApartment(apartment);
        //Assert.NotNull(result);
    }

    [Fact]
    public async Task CanReadApartment()
    {
        var id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc");
        var result = await _apartmentService.GetApartment(id);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CanUpdateApartment()
    {
        var apartment = new Apartments.Domain.Apartment();
        //var result = await _apartmentService.UpdateApartment(apartment);
        //Assert.NotNull(result);
    }

    [Fact]
    public async Task CanDeleteApartment()
    {
        var id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc");
        //var result = await _apartmentService.DeleteApartment(id);
        //Assert.True(result);
    }
}
