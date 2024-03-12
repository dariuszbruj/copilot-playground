using Apartments.Application.Common;
using Apartments.Application.Modules.AccountServices;
using Apartments.Application.Modules.AccountServices.Dtos;
using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Apartments.Infrastructure.Identity.Services;

public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager) 
    : IAccountService
{
    public async Task<Result> CreateAsync(CreateRequestCommand requestCommand, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var applicationUser = new User { UserName = requestCommand.UserName, Email = requestCommand.UserName };
        var identityResult = await userManager.CreateAsync(applicationUser, requestCommand.Password);

        return identityResult.Succeeded
            ? Result.Ok()
            : Result.Fail(identityResult.Errors.Select(e => e.Description));
    }

    public async Task<Result> LoginAsync(LoginRequestCommand requestCommand, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var user = await userManager.FindByNameAsync(requestCommand.UserName);

        if (user == null)
        {
            return Result.Fail(["UserNotFound"]);
        }

        var signInResult = await signInManager
            .CheckPasswordSignInAsync(user, requestCommand.Password, requestCommand.LockoutOnFailure);

        return signInResult.Succeeded
            ? Result.Ok()
            : signInResult.IsLockedOut
                ? Result.Fail(["UserLockOut"])
                : Result.Fail(["InvalidPassword"]);
    }
}
