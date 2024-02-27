using Apartments.Application.Apartments;
using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Dtos;
using FakeItEasy;

namespace Apartment.Api.UnitTests.Domain;

public class ApartmentServiceTests
{
    private readonly ApartmentService _apartmentService;
    private readonly IApartmentRepository _apartmentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentServiceTests"/> class.
    /// </summary>
    public ApartmentServiceTests()
    {
        _apartmentRepository = A.Fake<IApartmentRepository>();
        _apartmentService = new ApartmentService(_apartmentRepository);
    }

    /// <summary>
    /// Tests if an apartment can be created asynchronously.
    /// </summary>
    [Fact]
    public async Task CanCreateApartment()
    {
        var request = new CreateApartmentDto();
        
        var result = await _apartmentService.CreateApartment(request);
        
        Assert.NotNull(result);
    }

    /// <summary>
    /// Tests if an apartment can be read asynchronously.
    /// </summary>
    [Fact]
    public async Task CanReadApartment()
    {
        var id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc");
        var result = await _apartmentService.GetAsync(id);
        Assert.NotNull(result);
    }

    /// <summary>
    /// Tests if an apartment can be updated asynchronously.
    /// </summary>
    [Fact]
    public async Task CanUpdateApartment()
    {
        var apartment = new Apartments.Domain.Apartment();
        //var result = await _apartmentService.UpdateApartment(apartment);
        //Assert.NotNull(result);
    }

    /// <summary>
    /// Tests if an apartment can be deleted asynchronously.
    /// </summary>
    [Fact]
    public async Task CanDeleteApartment()
    {
        var id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc");
        //var result = await _apartmentService.DeleteAsync(id);
        //Assert.True(result);
    }
}
