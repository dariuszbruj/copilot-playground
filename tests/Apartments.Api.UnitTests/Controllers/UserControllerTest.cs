using Apartments.Domain.Services;
using Apartments.Domain.Services.AccountService;
using Apartments.Infrastructure.Identity.Models;
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

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestDto>._))
            .Returns(new CreateSuccessResult());

        // Act
        var response = await controller.Register(request);

        // Assert
        Assert.IsType<OkResult>(response);
        A.CallTo(() => accountServiceFake.CreateAsync(
            A<CreateRequestDto>.That.Matches(u => u.UserName == user.UserName && u.Password == password)))
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
        var user = new IdentityUser { UserName = username };
        var request = new RegisterRequest { UserName = username, Password = password };

        var errors = new string[] { "Error description" };
        var result = new CreateErrorResult() { ErrorMessage = errors };

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestDto>._))
            .Returns(result);

        // Act
        var response = await controller.Register(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(result.ErrorMessage, badRequestResult.Value);
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
        var user = new User { UserName = username };
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.CreateAsync(A<CreateRequestDto>._))
            .Returns(new CreateSuccessResult());

        // Act
        var response = await controller.Login(request);

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
        var user = new User { UserName = username };
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._))
            .Returns(new LoginSuccessResult());

        var expectedToken = "generated-jwt-token";
        A.CallTo(() => tokenGeneratorFake.GenerateToken(user.UserName))
            .Returns(expectedToken);

        // Act
        var response = await controller.Login(request);

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

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._))
            .Returns(new UserNotFoundResult());

        // Act
        var response = await controller.Login(request);

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
        var user = new User { UserName = username };
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => accountServiceFake.LoginAsync(A<LoginRequestDto>._))
            .Returns(new InvalidPasswordResult());

        // Act
        var response = await controller.Login(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UnauthorizedResult>(response.Result);
    }
    
}
