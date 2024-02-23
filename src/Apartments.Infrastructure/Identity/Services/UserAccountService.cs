using Apartments.Domain.Services.AccountService;
using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Apartments.Infrastructure.Identity.Services;

public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager) 
    : IAccountService
{
    public async Task<CreateResult> CreateAsync(CreateRequestDto requestDto)
    {
        var applicationUser = new User { UserName = requestDto.UserName, Email = requestDto.UserName };
        var identityResult = await userManager.CreateAsync(applicationUser, requestDto.Password);

        return identityResult.Succeeded
            ? new CreateSuccessResult()
            : new CreateErrorResult { ErrorMessage = identityResult.Errors.Select(e => e.Description) };
    }

    public async Task<LoginResult> 
        LoginAsync(LoginRequestDto requestDto)
    {
        var user = await userManager.FindByNameAsync(requestDto.UserName);

        if (user == null)
        {
            return new UserNotFoundResult();
        }

        var signInResult = await signInManager
            .CheckPasswordSignInAsync(user, requestDto.Password, requestDto.LockoutOnFailure);

        return signInResult.Succeeded
            ? new LoginSuccessResult()
            : signInResult.IsLockedOut
                ? new UserLockOutFoundResult()
                : new InvalidPasswordResult();
    }
}
