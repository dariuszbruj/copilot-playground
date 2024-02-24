using Apartments.Domain.Services;
using Apartments.Domain.Services.AccountService;
using Apartments.Domain.Services.AccountService.Dtos;
using Apartments.Domain.Services.AccountService.Models;
using Apartments.Domain.Services.AccountService.Results;
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
        var loginRequest = new LoginRequestDto(
            new UserName(request.UserName),
            new Password(request.Password),
            new LockoutOnFailure(true));

        var result = await accountService.LoginAsync(loginRequest, cancellationToken);

        if (result is not LoginSuccessResult)
        {
            return Unauthorized();
        }

        // TODO: username should be fetched from result
        var token = tokenGenerator.GenerateToken(request.UserName);

        return Ok(new LoginResponse { Token = token });

    }

    [HttpPost("register")]
    public async Task<ActionResult<CreateResult>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var createRequest = new CreateRequestDto(
            new UserName(request.UserName),
            new Password(request.Password));

        var result = await accountService.CreateAsync(createRequest, cancellationToken);

        // switch return on type
        return result switch
        {
            SuccessCreateResult => Ok(),
            ErrorCreateResult badRequest => BadRequest(badRequest.ErrorMessage),
            _ => BadRequest()
        };
    }
}
