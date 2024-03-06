using Apartments.Domain;
using Apartments.Domain.Services.AccountServices;
using Apartments.Domain.Services.AccountServices.Dtos;
using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Apartments.Infrastructure.Identity.Services;

public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager) 
    : IAccountService
{
    public async Task<Result> CreateAsync(CreateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var applicationUser = new User { UserName = requestDto.UserName, Email = requestDto.UserName };
        var identityResult = await userManager.CreateAsync(applicationUser, requestDto.Password);

        return identityResult.Succeeded
            ? Result.Ok()
            : Result.Fail(identityResult.Errors.Select(e => e.Description));
    }

    public async Task<Result> LoginAsync(LoginRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var user = await userManager.FindByNameAsync(requestDto.UserName);

        if (user == null)
        {
            return Result.Fail(["UserNotFound"]);
        }

        var signInResult = await signInManager
            .CheckPasswordSignInAsync(user, requestDto.Password, requestDto.LockoutOnFailure);

        return signInResult.Succeeded
            ? Result.Ok()
            : signInResult.IsLockedOut
                ? Result.Fail(["UserLockOut"])
                : Result.Fail(["InvalidPassword"]);
    }
}
