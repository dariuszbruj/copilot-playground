using Apartments.Application.Modules.Apartments;
using Apartments.Application.Modules.Apartments.Dtos;
using Apartments.Domain.Models;
using FakeItEasy;

namespace Apartment.Api.UnitTests.Domain;

public class ApartmentServiceTests
{
    private readonly List<Apartments.Domain.Models.Apartment> _apartments;
    private readonly ApartmentService _apartmentService;
    private readonly IApartmentRepository _apartmentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentServiceTests"/> class.
    /// </summary>
    public ApartmentServiceTests()
    {
        _apartmentRepository = A.Fake<IApartmentRepository>();
        
        // Create some fake apartments
        _apartments = new List<Apartments.Domain.Models.Apartment>
        {
            new()
            { 
                Id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc"), 
                Name = "Apartment 1", 
                Address = new Address()
                {
                    BuildingNo = "1", FlatNumber = "1", Street = "Street 1", City = "City 1", State = "State 1", ZipCode = "ZipCode 1"
                }},
            new()
            {
                Id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cd"),
                Name = "Apartment 2",
                Address = new Address()
                {
                    BuildingNo = "2", FlatNumber = "2", Street = "Street 2", City = "City 2", State = "State 2", ZipCode = "ZipCode 2"
                }
            }
        };

        // Set up the GetAsync method to return a fake apartment
        A.CallTo(() => _apartmentRepository.GetByIdAsync(A<Guid>._, A<CancellationToken>._))
            .ReturnsLazily((Guid id, CancellationToken _) => _apartments.First(a => a.Id == id));
        // Set up the AddAsync method to add a fake apartment
        A.CallTo(() => _apartmentRepository.AddAsync(A<Apartments.Domain.Models.Apartment>._, A<CancellationToken>._))
            .Invokes((Apartments.Domain.Models.Apartment apartment, CancellationToken _) => _apartments.Add(apartment));
        // Set up the DeleteAsync method to remove a fake apartment
        A.CallTo(() => _apartmentRepository.DeleteAsync(A<Guid>._, A<CancellationToken>._))
            .Invokes((Guid id, CancellationToken _) => _apartments.RemoveAll(a => a.Id == id));
        
        _apartmentService = new ApartmentService(_apartmentRepository);
    }

    /// <summary>
    /// Tests if an apartment can be created asynchronously.
    /// </summary>
    [Fact]
    public async Task CanCreateApartment()
    {
        // Arrange
        var request = new CreateApartmentCommand { Name = "Some New Apartment Name", Address = new ApartmentAddressDto() };
        
        // Act
        var result = await _apartmentService.CreateAsync(request);
        
        // Assert
        A.CallTo(() => _apartmentRepository.AddAsync(A<Apartments.Domain.Models.Apartment>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Contains(_apartments, a => a.Id == result.Value);
    }

    /// <summary>
    /// Tests if an apartment can be read asynchronously.
    /// </summary>
    [Fact]
    public async Task CanReadApartment()
    {
        // Arrange
        var guid = _apartments.First().Id;
        
        // Act
        var result = await _apartmentService.GetAsync(guid, CancellationToken.None);
        
        // Assert
        A.CallTo(() => _apartmentRepository.GetByIdAsync(guid, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(guid, result.Value.Id);
    }

    /// <summary>
    /// Tests if an apartment can be updated asynchronously.
    /// </summary>
    [Fact]
    public async Task CanUpdateApartment()
    {
        // Arrange
        var apartment = new UpdateApartmentCommand()
        {
            Id = Guid.Parse("d70070df-4bfb-4961-8092-4cd5085068cc"),
            Name = "Updated Name"
        };
        
        // Act
        var result = await _apartmentService.UpdateApartment(apartment);
        
        // Assert
        A.CallTo(() => _apartmentRepository.GetByIdAsync(apartment.Id, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _apartmentRepository.UpdateAsync(A<Apartments.Domain.Models.Apartment>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        Assert.NotNull(result);
    }

    /// <summary>
    /// Tests if an apartment can be deleted asynchronously.
    /// </summary>
    [Fact]
    public async Task CanDeleteApartment()
    {
        // Arrange
        var guid = _apartments.First().Id;
        
        // Act
        await _apartmentService.DeleteAsync(guid, CancellationToken.None);
        
        // Assert
        A.CallTo(() => _apartmentRepository.DeleteAsync(guid, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        Assert.DoesNotContain(_apartments, a => a.Id == guid);
    }
}
