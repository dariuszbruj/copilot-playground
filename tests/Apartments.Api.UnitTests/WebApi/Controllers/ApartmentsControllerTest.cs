using Apartments.Domain;
using Apartments.Domain.Services.Apartments;
using Apartments.Domain.Services.Apartments.Dtos;
using Apartments.WebApi.Controllers;
using Apartments.WebApi.Requests;
using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Apartment.Api.UnitTests.WebApi.Controllers
{
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
            var okResult = Assert.IsType<Ok<IEnumerable<ApartmentDto>>>(response);
            var resultValue = Assert.IsAssignableFrom<IEnumerable<ApartmentDto>>(okResult.Value);
            Assert.Equal(apartments, resultValue);
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
            Assert.IsType<NotFound<IEnumerable<string>>>(response);
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
            var okResult = Assert.IsType<Ok<ApartmentDto>>(response);
            var resultValue = Assert.IsType<ApartmentDto>(okResult.Value);
            Assert.Equal(apartment, resultValue);
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
            Assert.IsType<NotFound<IEnumerable<string>>>(response);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnCreatedAtActionResult_WhenApartmentIsCreated()
        {
            // Arrange
            var apartmentServiceFake = A.Fake<IApartmentService>();
            var controller = new ApartmentsController(apartmentServiceFake);

            var apartmentId = Guid.NewGuid();
            var request = new ApartmentRequest { Name = "Apartment 1" };
            var dto = new CreateApartmentDto { Name = request.Name, Address = new CreateApartmentAddressDto() };

            A.CallTo(() => apartmentServiceFake.CreateApartment(dto, A<CancellationToken>._))
                .Returns(Result<Guid>.Ok(apartmentId));

            // Act
            var response = await controller.PostAsync(request, CancellationToken.None);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtRoute>(response);
            Assert.Equal(nameof(controller.GetAsync), createdAtActionResult.RouteName);
            Assert.Equal(apartmentId, createdAtActionResult.RouteValues["id"]);
            //Assert.Null(createdAtActionResult.Value);
        }

        [Fact]
        public async Task PostAsync_ShouldReturnBadRequestResult_WhenApartmentCreationFails()
        {
            // Arrange
            var apartmentServiceFake = A.Fake<IApartmentService>();
            var controller = new ApartmentsController(apartmentServiceFake);

            var request = new ApartmentRequest { Name = "Apartment 1" };
            var dto = new CreateApartmentDto { Name = request.Name, Address = new CreateApartmentAddressDto()  };

            var errors = new[] { "Error description" };
            var result = Result<Guid>.Fail(errors);

            A.CallTo(() => apartmentServiceFake.CreateApartment(dto, A<CancellationToken>._))
                .Returns(result);

            // Act
            var response = await controller.PostAsync(request, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<IEnumerable<string>>>(response);
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
            var dto = new UpdateApartmentDto { Id = apartmentId, Name = request.Name };

            A.CallTo(() => apartmentServiceFake.UpdateApartment(dto, A<CancellationToken>._))
                .Returns(Result.Ok());

            // Act
            var response = await controller.PutAsync(apartmentId, request, CancellationToken.None);

            // Assert
            Assert.IsType<NoContent>(response);
        }

        [Fact]
        public async Task PutAsync_ShouldReturnNotFoundResult_WhenApartmentToUpdateDoesNotExist()
        {
            // Arrange
            var apartmentServiceFake = A.Fake<IApartmentService>();
            var controller = new ApartmentsController(apartmentServiceFake);

            var apartmentId = Guid.NewGuid();
            var request = new UpdateApartmentRequest { Name = "Updated Apartment" };
            var dto = new UpdateApartmentDto { Id = apartmentId, Name = request.Name };

            A.CallTo(() => apartmentServiceFake.UpdateApartment(dto, A<CancellationToken>._))
                .Returns(Result.Fail(["NotFound"]));

            // Act
            var response = await controller.PutAsync(apartmentId, request, CancellationToken.None);

            // Assert
            Assert.IsType<NotFound<IEnumerable<string>>>(response);
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
            Assert.IsType<NoContent>(response);
            A.CallTo(() => apartmentServiceFake.DeleteAsync(apartmentId, A<CancellationToken>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}
