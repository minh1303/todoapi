using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequest request)
    {
       var newUser =  await authService.RegisterAsync(request.Email, request.Password);
       if (newUser == null) return BadRequest("Email already exists");
       return Ok();
       
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token =  await authService.LoginAsync(request.Email, request.Password);
        if (token == null) return Unauthorized("Username or password is incorrect");
        return Ok(token);
    }
}

    
public record LoginRequest(string Email, string Password);
