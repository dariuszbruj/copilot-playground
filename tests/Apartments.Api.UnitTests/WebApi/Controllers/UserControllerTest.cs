using Apartments.Domain.Services;
using Apartments.Domain.Services.AccountService;
using Apartments.Domain.Services.AccountService.Dtos;
using Apartments.Domain.Services.AccountService.Results;
using Apartments.WebApi.Controllers;
using Apartments.WebApi.Requests;
using Apartments.WebApi.Response;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apartment.Api.UnitTests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task Register_ShouldCreateUserAndReturnOkResult()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new IdentityUser { UserName = username };
        var request = new RegisterRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestDto>._, A<CancellationToken>._))
            .Returns(new SuccessCreateResult());

        // Act
        var response = await controller.RegisterAsync(request);

        // Assert
        Assert.IsType<OkResult>(response.Result);
        A.CallTo(() => accountServiceFake.CreateAsync(
            A<CreateRequestDto>.That.Matches(u => u.UserName == user.UserName && u.Password == password), A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequestWhenUserCreationFails()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var request = new RegisterRequest { UserName = username, Password = password };

        var errors = new[] { "Error description" };
        var result = new ErrorCreateResult() { ErrorMessage = errors };

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestDto>._, A<CancellationToken>._))
            .Returns(result);

        // Act
        var response = await controller.RegisterAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response.Result);
        Assert.Equal(result.ErrorMessage, badRequestResult.Value);
    }

    private sealed record SomeInvalidCreateResult : CreateResult;

    [Fact]
    public async Task Register_ShouldReturnBadRequestWhenThereInvalidResponseOccured()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var request = new RegisterRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestDto>._, A<CancellationToken>._))
            .Returns(new SomeInvalidCreateResult());

        // Act
        var response = await controller.RegisterAsync(request);

        // Assert
        Assert.IsType<BadRequestResult>(response.Result);
        A.CallTo(() => accountServiceFake.CreateAsync(
            A<CreateRequestDto>.That.Matches(u => u.UserName == username && u.Password == password), A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Login_ShouldReturnOkObjectResult_WhenCredentialsAreValid()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword"; ;
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._, A<CancellationToken>._))
            .Returns(new LoginSuccessResult());

        // Act
        var response = await controller.LoginAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<ActionResult<LoginResponse>>(response);
    }

    [Fact]
    public async Task Login_ShouldReturnOkObjectResultWithGeneratedJwtToken_WhenCredentialsAreValid()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._, A<CancellationToken>._))
            .Returns(new LoginSuccessResult());

        var expectedToken = "generated-jwt-token";
        A.CallTo(() => tokenGeneratorFake.GenerateToken(username))
            .Returns(expectedToken);

        // Act
        var response = await controller.LoginAsync(request);

        // Assert
        Assert.NotNull(response);
        var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);
        var loginResponse = Assert.IsType<LoginResponse>(okObjectResult.Value);
        Assert.Equal(expectedToken, loginResponse.Token);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._, A<CancellationToken>._))
            .Returns(new UserNotFoundLoginResult());

        // Act
        var response = await controller.LoginAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UnauthorizedResult>(response.Result);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var accountServiceFake = A.Fake<IAccountService>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(accountServiceFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._, A<CancellationToken>._))
            .Returns(new InvalidPasswordLoginResult());

        // Act
        var response = await controller.LoginAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UnauthorizedResult>(response.Result);
    }
    
}
