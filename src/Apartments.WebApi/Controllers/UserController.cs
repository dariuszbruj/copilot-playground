using Apartments.Domain.Services;
using Apartments.Infrastructure.Identity.Models;
using Apartments.WebApi.Requests;
using Apartments.WebApi.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("user")]
public class UserController(UserManager<User> userManager, SignInManager<User> signInManager, ITokenGenerator tokenGenerator)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded || user.UserName == null)
        {
            return Unauthorized();
        }

        var token = tokenGenerator.GenerateToken(user.UserName);
            
        return Ok(new LoginResponse { Token = token });

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new User { UserName = request.UserName, Email = request.UserName};
        var result = await userManager.CreateAsync(user, request.Password);

        return result.Succeeded 
            ? Ok() : BadRequest(result.Errors);
    }
}
