using Apartments.Application.Modules.AccountServices;
using Apartments.Application.Modules.AccountServices.Dtos;
using Apartments.Application.Modules.AccountServices.Models;
using Apartments.Application.Modules.Tokens;
using Apartments.WebApi.Requests;
using Apartments.WebApi.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

/// <summary>
/// Controller for managing user authentication.
/// </summary>
/// <param name="accountService">Service for managing user accounts.</param>
/// <param name="tokenGenerator">Service for generating JWT tokens.</param>
[ApiController]
[AllowAnonymous]
[Route("user")]
public class UserController(IAccountService accountService, ITokenGenerator tokenGenerator)
    : ControllerBase
{
    /// <summary>
    /// Logs in the user and returns a token.
    /// </summary>
    /// <param name="request">The login request containing the user's credentials.</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>A JWT token if the login is successful; Unauthorized if the login fails.</returns>
    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration request containing the user's credentials.</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
    /// <returns>Ok if the registration is successful; BadRequest if the registration fails.</returns>
    [HttpPost("register")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
