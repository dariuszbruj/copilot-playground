using Apartments.Infrastructure.Identity.Models;
using Apartments.WebApi.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

        return result.Succeeded 
            ? Ok() : Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var user = new User { UserName = request.Username, Email = request.Username};
        var result = await userManager.CreateAsync(user, request.Password);

        return result.Succeeded 
            ? Ok() : BadRequest(result.Errors);
    }
}
