using Apartments.Infrastructure.Identity.Models;
using Apartments.WebApi.Controllers;
using FakeItEasy;
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
        var controller = new UserController(userManagerFake, signInManagerFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new IdentityUser { UserName = username };

        A.CallTo(() => userManagerFake.CreateAsync(A<User>._, A<string>._))
            .Returns(IdentityResult.Success);

        // Act
        var response = await controller.Register(username, password);

        // Assert
        Assert.IsType<OkResult>(response);
        A.CallTo(() => userManagerFake.CreateAsync(
            A<User>.That.Matches(u => u.UserName == user.UserName && u.Email == user.Email),
            A<string>.That.Matches(p => p == password)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequestWhenUserCreationFails()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var controller = new UserController(userManagerFake, signInManagerFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new IdentityUser { UserName = username };

        var errors = new List<IdentityError> { new() { Description = "Error description" } };
        var result = IdentityResult.Failed([.. errors]);

        A.CallTo(() => userManagerFake.CreateAsync(A<User>._, A<string>._))
            .Returns(result);

        // Act
        var response = await controller.Register(username, password);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal(result.Errors, badRequestResult.Value);
        A.CallTo(() => userManagerFake.CreateAsync(
            A<User>.That.Matches(u => u.UserName == user.UserName && u.Email == user.Email),
            A<string>.That.Matches(p => p == password)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Login_ShouldReturnOkResult_WhenCredentialsAreValid()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var controller = new UserController(userManagerFake, signInManagerFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new User { UserName = username };

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns(user);
        A.CallTo(() => signInManagerFake.PasswordSignInAsync(user, password, false, false))
            .Returns(SignInResult.Success);

        // Act
        var response = await controller.Login(username, password);

        // Assert
        Assert.IsType<OkResult>(response);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var controller = new UserController(userManagerFake, signInManagerFake);

        const string username = "testuser";
        const string password = "testpassword";

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns((User?)null);

        // Act
        var response = await controller.Login(username, password);

        // Assert
        Assert.IsType<UnauthorizedResult>(response);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var userManagerFake = A.Fake<UserManager<User>>();
        var signInManagerFake = A.Fake<SignInManager<User>>();
        var controller = new UserController(userManagerFake, signInManagerFake);

        const string username = "testuser";
        const string password = "testpassword";
        var user = new User { UserName = username };

        A.CallTo(() => userManagerFake.FindByNameAsync(username))
            .Returns(user);
        A.CallTo(() => signInManagerFake.PasswordSignInAsync(user, password, false, false))
            .Returns(SignInResult.Failed);

        // Act
        var response = await controller.Login(username, password);

        // Assert
        Assert.IsType<UnauthorizedResult>(response);
    }
}
