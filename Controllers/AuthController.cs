using Microsoft.AspNetCore.Mvc;
using SalesApi.Authentication;

namespace SalesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto loginDto)
    {
        var login = _authService.LoginAsync(loginDto.Email, loginDto.Password);

        return Ok(new { Token = login });
    }

}
