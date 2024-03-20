using Apartments.Application.Modules.Utilities;
using Apartments.Domain.Models;
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
        var utilityMeasurementRequest = new UtilityMeasurementRequest
        {
            ApartmentId = Guid.NewGuid(),
            MeasurementDate = DateTime.Now,
            UtilityUsage = 100.5m,
            UtilityType = UtilityTypeRequest.Water
        };

        // Act
        await controller.InsertUtilityValues(utilityMeasurementRequest);

        // Assert
        A.CallTo(() => fakeService.InsertUtilityValues(A<UtilityMeasurement>.That.Matches(um =>
            um.ApartmentId == utilityMeasurementRequest.ApartmentId &&
            um.MeasurementDate == utilityMeasurementRequest.MeasurementDate &&
            um.UtilityUsage == utilityMeasurementRequest.UtilityUsage &&
            um.UtilityType == (UtilityType)Enum.Parse(typeof(UtilityType), utilityMeasurementRequest.UtilityType.ToString())
        ))).MustHaveHappenedOnceExactly();
    }
}
