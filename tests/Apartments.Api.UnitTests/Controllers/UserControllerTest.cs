using Apartments.Domain.Services;
using Apartments.Infrastructure.Identity.Models;
using Apartments.WebApi;
using Apartments.WebApi.Controllers;
using Apartments.WebApi.Requests;
using Apartments.WebApi.Response;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Apartment.Api.UnitTests.Controllers;

public class UserControllerTests
{
    [Fact]
    public async Task Register_ShouldCreateUserAndReturnOkResult()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(userManagerFake, signInManagerFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new IdentityUser { UserName = username };
        var request = new RegisterRequest { UserName = username, Password = password };

        A.CallTo(() => userManagerFake.CreateAsync(A<User>._, A<string>._))
            .Returns(IdentityResult.Success);

        // Act
        var response = await controller.Register(request);

        // Assert
        Assert.IsType<OkResult>(response);
        A.CallTo(() => userManagerFake.CreateAsync(
            A<User>.That.Matches(u => u.UserName == user.UserName),
            A<string>.That.Matches(p => p == password)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequestWhenUserCreationFails()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(userManagerFake, signInManagerFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new IdentityUser { UserName = username };
        var request = new RegisterRequest { UserName = username, Password = password };

        var errors = new List<IdentityError> { new() { Description = "Error description" } };
        var result = IdentityResult.Failed([.. errors]);

        A.CallTo(() => userManagerFake.CreateAsync(A<User>._, A<string>._))
            .Returns(result);

        // Act
        var response = await controller.Register(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(result.Errors, badRequestResult.Value);
        A.CallTo(() => userManagerFake.CreateAsync(
            A<User>.That.Matches(u => u.UserName == user.UserName),
            A<string>.That.Matches(p => p == password)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Login_ShouldReturnOkObjectResult_WhenCredentialsAreValid()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(userManagerFake, signInManagerFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new User { UserName = username };
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns(user);
        A.CallTo(() => signInManagerFake.PasswordSignInAsync(username, password, false, false))
            .Returns(SignInResult.Success);

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
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(userManagerFake, signInManagerFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new User { UserName = username };
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns(user);
        A.CallTo(() => signInManagerFake.CheckPasswordSignInAsync(user, password, false))
            .Returns(SignInResult.Success);

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
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(userManagerFake, signInManagerFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns((User?)null);

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
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var tokenGeneratorFake = A.Fake<ITokenGenerator>();
        var controller = new UserController(userManagerFake, signInManagerFake, tokenGeneratorFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new User { UserName = username };
        var request = new LoginRequest { UserName = username, Password = password };

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns(user);
        A.CallTo(() => signInManagerFake.PasswordSignInAsync(user, password, false, false))
            .Returns(SignInResult.Failed);

        // Act
        var response = await controller.Login(request);

        // Assert
        Assert.NotNull(response);
        Assert.IsType<UnauthorizedResult>(response.Result);
    }
    
}
