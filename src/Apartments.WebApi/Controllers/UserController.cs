using Apartments.Application.Modules.AccountServices;
using Apartments.Application.Modules.AccountServices.Dtos;
using Apartments.Application.Modules.AccountServices.Models;
using Apartments.Application.Modules.Tokens;
using Apartments.WebApi.Requests;
using Apartments.WebApi.Response;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("user")]
public class UserController(IAccountService accountService, ITokenGenerator tokenGenerator)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var loginRequest = new LoginRequestCommand(
            new UserName(request.UserName),
            new Password(request.Password),
            new LockoutOnFailure(true));

        var result = await accountService.LoginAsync(loginRequest, cancellationToken);

        if (result.IsFailed)
        {
            return Unauthorized();
        }

        // TODO: username should be fetched from result
        var token = tokenGenerator.GenerateToken(request.UserName);

        return Ok(new LoginResponse { Token = token });

    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var createRequest = new CreateRequestCommand(
            new UserName(request.UserName),
            new Password(request.Password));

        var result = await accountService.CreateAsync(createRequest, cancellationToken);

        // switch return on type
        return result.IsSuccess switch
        {
            true => Ok(),
            false => BadRequest(result.Errors)
        };
    }
}
