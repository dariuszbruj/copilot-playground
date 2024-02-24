using Apartments.Domain.Services.AccountService;
using Apartments.Domain.Services.AccountService.Dtos;
using Apartments.Domain.Services.AccountService.Results;
using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Apartments.Infrastructure.Identity.Services;

public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager) 
    : IAccountService
{
    public async Task<CreateResult> CreateAsync(CreateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var applicationUser = new User { UserName = requestDto.UserName, Email = requestDto.UserName };
        var identityResult = await userManager.CreateAsync(applicationUser, requestDto.Password);

        return identityResult.Succeeded
            ? new SuccessCreateResult()
            : new ErrorCreateResult { ErrorMessage = identityResult.Errors.Select(e => e.Description) };
    }

    public async Task<LoginResult> LoginAsync(LoginRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var user = await userManager.FindByNameAsync(requestDto.UserName);

        if (user == null)
        {
            return new UserNotFoundLoginResult();
        }

        var signInResult = await signInManager
            .CheckPasswordSignInAsync(user, requestDto.Password, requestDto.LockoutOnFailure);

        return signInResult.Succeeded
            ? new LoginSuccessResult()
            : signInResult.IsLockedOut
                ? new UserLockOutFoundLoginResult()
                : new InvalidPasswordLoginResult();
    }
}
