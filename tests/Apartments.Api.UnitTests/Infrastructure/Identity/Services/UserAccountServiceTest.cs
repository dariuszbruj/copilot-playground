using Apartments.Domain;
using Apartments.Domain.Services.AccountServices.Dtos;
using Apartments.Domain.Services.AccountServices.Models;
using Apartments.Infrastructure.Identity.Models;
using Apartments.Infrastructure.Identity.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;

namespace Apartment.Api.UnitTests.Infrastructure.Identity.Services
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldReturnSuccessCreateResult_WhenUserCreationSucceeds()
        {
            // Arrange
            var userManagerFake = A.Fake<UserManager<User>>();
            var signInManagerFake = A.Fake<SignInManager<User>>();
            var accountService = new AccountService(userManagerFake, signInManagerFake);

            var requestDto = new CreateRequestDto(new UserName("testuser"), new Password("testpassword"));

            A.CallTo(() => userManagerFake.CreateAsync(A<User>._, A<string>._))
                .Returns(IdentityResult.Success);

            // Act
            var result = await accountService.CreateAsync(requestDto);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnErrorCreateResult_WhenUserCreationFails()
        {
            // Arrange
            var userManagerFake = A.Fake<UserManager<User>>();
            var signInManagerFake = A.Fake<SignInManager<User>>();
            var accountService = new AccountService(userManagerFake, signInManagerFake);

            var requestDto = new CreateRequestDto(new UserName("testuser"), new Password("testpassword"));

            var identityErrors = new[]
            {
                new IdentityError { Description = "Error 1" },
                new IdentityError { Description = "Error 2" }
            };

            A.CallTo(() => userManagerFake.CreateAsync(A<User>._, A<string>._))
                .Returns(IdentityResult.Failed(identityErrors));

            // Act
            var result = await accountService.CreateAsync(requestDto);

            // Assert
            var errorResult = Assert.IsType<Result>(result);
            Assert.True(result.IsFailed);
            Assert.Equal(identityErrors.Select(e => e.Description), errorResult.Errors);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUserNotFoundLoginResult_WhenUserDoesNotExist()
        {
            // Arrange
            var userManagerFake = A.Fake<UserManager<User>>();
            var signInManagerFake = A.Fake<SignInManager<User>>();
            var accountService = new AccountService(userManagerFake, signInManagerFake);

            var requestDto = new LoginRequestDto(new UserName("testuser"), new Password("testpassword"),
                new LockoutOnFailure(true));

            A.CallTo(() => userManagerFake.FindByNameAsync(A<string>._))
                .Returns((User?)null);

            // Act
            var result = await accountService.LoginAsync(requestDto);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsFailed);
            Assert.Equal(["UserNotFound"], result.Errors);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnLoginSuccessResult_WhenCredentialsAreValid()
        {
            // Arrange
            var userManagerFake = A.Fake<UserManager<User>>();
            var signInManagerFake = A.Fake<SignInManager<User>>();
            var accountService = new AccountService(userManagerFake, signInManagerFake);

            var requestDto = new LoginRequestDto(new UserName("testuser"), new Password("testpassword"),
                new LockoutOnFailure(true));

            var user = new User { UserName = "testuser" };

            A.CallTo(() => userManagerFake.FindByNameAsync(A<string>._))
                .Returns(user);

            A.CallTo(() => signInManagerFake.CheckPasswordSignInAsync(A<User>._, A<string>._, A<bool>._))
                .Returns(SignInResult.Success);

            // Act
            var result = await accountService.LoginAsync(requestDto);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnUserLockOutFoundLoginResult_WhenUserIsLockedOut()
        {
            // Arrange
            var userManagerFake = A.Fake<UserManager<User>>();
            var signInManagerFake = A.Fake<SignInManager<User>>();
            var accountService = new AccountService(userManagerFake, signInManagerFake);

            var requestDto = new LoginRequestDto(new UserName("testuser"), new Password("testpassword"),
                new LockoutOnFailure(true));

            var user = new User { UserName = "testuser" };

            A.CallTo(() => userManagerFake.FindByNameAsync(A<string>._))
                .Returns(user);

            A.CallTo(() => signInManagerFake.CheckPasswordSignInAsync(A<User>._, A<string>._, A<bool>._))
                .Returns(SignInResult.LockedOut);

            // Act
            var result = await accountService.LoginAsync(requestDto);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsFailed);
            Assert.Equal(["UserLockOut"], result.Errors);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnInvalidPasswordLoginResult_WhenCredentialsAreInvalid()
        {
            // Arrange
            var userManagerFake = A.Fake<UserManager<User>>();
            var signInManagerFake = A.Fake<SignInManager<User>>();
            var accountService = new AccountService(userManagerFake, signInManagerFake);

            var requestDto = new LoginRequestDto(new UserName("testuser"), new Password("testpassword"),
                new LockoutOnFailure(true));

            var user = new User { UserName = "testuser" };

            A.CallTo(() => userManagerFake.FindByNameAsync(A<string>._))
                .Returns(user);

            A.CallTo(() => signInManagerFake.CheckPasswordSignInAsync(A<User>._, A<string>._, A<bool>._))
                .Returns(SignInResult.Failed);

            // Act
            var result = await accountService.LoginAsync(requestDto);

            // Assert
            Assert.IsType<Result>(result);
            Assert.True(result.IsFailed);
            Assert.Equal(["InvalidPassword"], result.Errors);
        }
    }
}
