using Apartments.Application.Common;
using Apartments.Application.Modules.Apartments;
using Apartments.Application.Modules.Apartments.Dtos;
using Apartments.WebApi.Controllers;
using Apartments.WebApi.Requests;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace Apartment.Api.UnitTests.WebApi.Controllers;

public class ApartmentsControllerTests
{
    [Fact]
    public async Task GetAsync_ShouldReturnOkResult_WhenApartmentsExist()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartments = new List<ApartmentDto>
        {
            new () { Id = Guid.NewGuid(), Name = "Apartment 1" },
            new () { Id = Guid.NewGuid(), Name = "Apartment 2" }
        };

        A.CallTo(() => apartmentServiceFake.GetAsync(A<CancellationToken>._))
            .Returns(Result<IEnumerable<ApartmentDto>>.Ok(apartments));

        // Act
        var response = await controller.GetAsync(CancellationToken.None);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<ApartmentDto>>>(response);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ApartmentDto>>(okResult.Value);

        Assert.Equal(apartments.Count, returnValue.Count);
        for (var i = 0; i < apartments.Count; i++)
        {
            Assert.Equal(apartments[i].Id, returnValue[i].Id);
            Assert.Equal(apartments[i].Name, returnValue[i].Name);
        }  
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFoundResult_WhenNoApartmentsExist()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        A.CallTo(() => apartmentServiceFake.GetAsync(A<CancellationToken>._))
            .Returns(Result<IEnumerable<ApartmentDto>>.Fail(["NotFound"]));

        // Act
        var response = await controller.GetAsync(CancellationToken.None);

        // Assert
        var result = Assert.IsType<ActionResult<IEnumerable<ApartmentDto>>>(response);
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnOkResult_WhenApartmentExists()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartmentId = Guid.NewGuid();
        var apartment = new ApartmentDto { Id = apartmentId, Name = "Apartment 1" };

        A.CallTo(() => apartmentServiceFake.GetAsync(apartmentId, A<CancellationToken>._))
            .Returns(Result<ApartmentDto>.Ok(apartment));

        // Act
        var response = await controller.GetAsync(apartmentId, CancellationToken.None);

        // Assert
        var result = Assert.IsType<ActionResult<ApartmentDto>>(response);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(apartment, okResult.Value);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNotFoundResult_WhenApartmentDoesNotExist()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartmentId = Guid.NewGuid();

        A.CallTo(() => apartmentServiceFake.GetAsync(apartmentId, A<CancellationToken>._))
            .Returns(Result<ApartmentDto>.Fail(["NotFound"]));

        // Act
        var response = await controller.GetAsync(apartmentId, CancellationToken.None);

        // Assert
        var result = Assert.IsType<ActionResult<ApartmentDto>>(response);
        Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(default, response.Value);
    }

    [Fact]
    public async Task PostAsync_ShouldReturnCreatedAtActionResult_WhenApartmentIsCreated()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartmentId = Guid.NewGuid();
        var request = new ApartmentRequest { Name = "Apartment 1" };
        var dto = new CreateApartmentCommand { Name = request.Name, Address = new CreateApartmentAddressDto() };

        A.CallTo(() => apartmentServiceFake.CreateAsync(dto, A<CancellationToken>._))
            .Returns(Result<Guid>.Ok(apartmentId));

        // Act
        var response = await controller.PostAsync(request, CancellationToken.None);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtRouteResult>(response);
        Assert.Equal(nameof(controller.GetAsync), createdAtActionResult.RouteName);
        Assert.NotNull(createdAtActionResult);
        Assert.Equal(apartmentId, createdAtActionResult.RouteValues?["id"]);
        Assert.Null(createdAtActionResult.Value);
    }

    [Fact]
    public async Task PostAsync_ShouldReturnBadRequestResult_WhenApartmentCreationFails()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var request = new ApartmentRequest { Name = "Apartment 1" };
        var dto = new CreateApartmentCommand { Name = request.Name, Address = new CreateApartmentAddressDto()  };

        var errors = new[] { "Error description" };
        var result = Result<Guid>.Fail(errors);

        A.CallTo(() => apartmentServiceFake.CreateAsync(dto, A<CancellationToken>._))
            .Returns(result);

        // Act
        var response = await controller.PostAsync(request, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(result.Errors, badRequestResult.Value);
    }

    [Fact]
    public async Task PutAsync_ShouldReturnNoContentResult_WhenApartmentIsUpdated()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartmentId = Guid.NewGuid();
        var request = new UpdateApartmentRequest { Name = "Updated Apartment" };
        var dto = new UpdateApartmentCommand { Id = apartmentId, Name = request.Name };

        A.CallTo(() => apartmentServiceFake.UpdateApartment(dto, A<CancellationToken>._))
            .Returns(Result.Ok());

        // Act
        var response = await controller.PutAsync(apartmentId, request, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public async Task PutAsync_ShouldReturnNotFoundResult_WhenApartmentToUpdateDoesNotExist()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartmentId = Guid.NewGuid();
        var request = new UpdateApartmentRequest { Name = "Updated Apartment" };
        var dto = new UpdateApartmentCommand { Id = apartmentId, Name = request.Name };

        A.CallTo(() => apartmentServiceFake.UpdateApartment(dto, A<CancellationToken>._))
            .Returns(Result.Fail(["NotFound"]));

        // Act
        var response = await controller.PutAsync(apartmentId, request, CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContentResult_WhenApartmentIsDeleted()
    {
        // Arrange
        var apartmentServiceFake = A.Fake<IApartmentService>();
        var controller = new ApartmentsController(apartmentServiceFake);

        var apartmentId = Guid.NewGuid();

        // Act
        var response = await controller.DeleteAsync(apartmentId, CancellationToken.None);

        // Assert
        Assert.IsType<NoContentResult>(response);
        A.CallTo(() => apartmentServiceFake.DeleteAsync(apartmentId, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }
}
