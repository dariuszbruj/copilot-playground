using Apartments.Application.Common;
using Apartments.Application.Modules.AccountServices;
using Apartments.Application.Modules.AccountServices.Dtos;
using Apartments.Application.Modules.Tokens;
using Apartments.WebApi.Controllers;
using Apartments.WebApi.Requests;
using Apartments.WebApi.Response;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apartment.Api.UnitTests.WebApi.Controllers;

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

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestCommand>._, A<CancellationToken>._))
            .Returns(Result.Ok());

        // Act
        var response = await controller.RegisterAsync(request);

        // Assert
        Assert.IsType<OkResult>(response);
        A.CallTo(() => accountServiceFake.CreateAsync(
            A<CreateRequestCommand>.That.Matches(u => u.UserName == user.UserName && u.Password == password), A<CancellationToken>._))
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
        var result = Result.Fail(errors);

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestCommand>._, A<CancellationToken>._))
            .Returns(result);

        // Act
        var response = await controller.RegisterAsync(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(result.Errors, badRequestResult.Value);
    }
    
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

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestCommand>._, A<CancellationToken>._))
            .Returns(Result.Fail([]));

        // Act
        var response = await controller.RegisterAsync(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(response);
        A.CallTo(() => accountServiceFake.CreateAsync(
            A<CreateRequestCommand>.That.Matches(u => u.UserName == username && u.Password == password), A<CancellationToken>._))
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
        const string password = "testpassword";
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestCommand>._, A<CancellationToken>._))
            .Returns(Result.Ok());

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

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestCommand>._, A<CancellationToken>._))
            .Returns(Result.Ok());

        const string expectedToken = "generated-jwt-token";
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

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestCommand>._, A<CancellationToken>._))
            .Returns(Result.Fail([ "UserNotFound" ]));

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

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestCommand>._, A<CancellationToken>._))
            .Returns(Result.Fail([ "InvalidPassword" ]));

        // Act
        var response = await controller.LoginAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UnauthorizedResult>(response.Result);
    }
    
}
