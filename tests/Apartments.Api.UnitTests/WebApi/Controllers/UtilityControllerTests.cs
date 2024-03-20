using Apartments.Application.Modules.Utilities;
using Apartments.Application.Modules.Utilities.Dtos;
using Apartments.WebApi.Controllers;
using Apartments.WebApi.Requests;
using FakeItEasy;

namespace Apartment.Api.UnitTests.WebApi.Controllers;

public class UtilityControllerTests
{
    [Fact]
    public async Task InsertUtilityValues_CallsServiceWithCorrectValues()
    {
        // Arrange
        var fakeService = A.Fake<IUtilityService>();
        var controller = new UtilityController(fakeService);
        var apartmentGuid = new Guid("d3f3e3e3-3e3e-3e3e-3e3e-3e3e3e3e3e3e");
        var utilityMeasurementRequest = new UtilityMeasurementRequest
        {
            MeasurementDate = DateTime.Now,
            UtilityUsage = 100.5m,
            UtilityType = UtilityTypeRequest.Water
        };

        // Act
        await controller.InsertUtilityValues(apartmentGuid, utilityMeasurementRequest);

        // Assert
        A.CallTo(() => fakeService.InsertUtilityValues(A<UtilityMeasurementDto>.That.Matches(um =>
            um.ApartmentId == apartmentGuid &&
            um.MeasurementDate == utilityMeasurementRequest.MeasurementDate &&
            um.UtilityUsage == utilityMeasurementRequest.UtilityUsage &&
            um.UtilityType == (UtilityTypeDto)Enum.Parse(typeof(UtilityTypeDto), utilityMeasurementRequest.UtilityType.ToString())
        ))).MustHaveHappenedOnceExactly();
    }
}
