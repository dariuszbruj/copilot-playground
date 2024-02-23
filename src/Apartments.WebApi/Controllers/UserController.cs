using Apartments.Domain.Services;
using Apartments.Domain.Services.AccountService;
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
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var loginRequest = new LoginRequestDto(
            new UserName(request.UserName),
            new Password(request.Password),
            new LockoutOnFailure(true));

        var result = await accountService.LoginAsync(loginRequest);

        if (result is not LoginSuccessResult)
        {
            return Unauthorized();
        }

        // TODO: username should be fetched from result
        var token = tokenGenerator.GenerateToken(request.UserName);

        return Ok(new LoginResponse { Token = token });

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var createRequest = new CreateRequestDto(
            new UserName(request.UserName),
            new Password(request.Password));

        var result = await accountService.CreateAsync(createRequest);

        // switch return on type
        return result switch
        {
            CreateSuccessResult => Ok(),
            CreateErrorResult badRequest => BadRequest(badRequest.ErrorMessage),
            _ => BadRequest()
        };
    }
}
