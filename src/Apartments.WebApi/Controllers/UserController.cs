using Apartments.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Apartments.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(UserManager<User> userManager, SignInManager<User> signInManager)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            return Unauthorized();
        }

        var result = await signInManager.PasswordSignInAsync(user, password, false, false);
        if (result.Succeeded)
        {
            return Ok();
        }
        
        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string password)
    {
        var user = new User { UserName = username, Email = username};
        var result = await userManager.CreateAsync(user, password);

        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }
}
