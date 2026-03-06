using Microsoft.AspNetCore.Mvc;
using SalesApi.Dto.User;
using SalesApi.Services;

namespace SalesApi.Controllers;

[ApiController]
[Route("Api/[Controller]")]

public class UserController : ControllerBase
{
    private readonly UserServices _userServices;

    public UserController(UserServices services)
    {
        _userServices = services;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
    {
        var user = await _userServices.CreateUserAsync(userDto);
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto userDto)
    {
        var user = await _userServices.UpdateUserAsync(userDto);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        var user = await _userServices.DeleteUserAsync(id);
        return Ok(user);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(int id)
    {
        var user = await _userServices.GetUserByIdAsync(id);
        return Ok(user);
    }

}
