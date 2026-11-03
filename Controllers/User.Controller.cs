
using Microsoft.AspNetCore.Mvc;
using WalletApi.Dtos;
using WalletApi.Services;

namespace WalletApi.Controllers;

[ApiController]
[Route("api/v1/users")]

public class UserController : ControllerBase

{
    private readonly IUserService _userService;
    public UserController(IUserService userService)

    {
        _userService = userService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRequest userRequest)

    {
        var user = await _userService.CreateUserAsync(userRequest);
        return Created($"api/v1/users/{user.Id}", user);
    }

    [HttpGet("{id}/wallet")]
    public async Task<IActionResult> GetBalace([FromRoute] UserBalanceRequest request)
    {
        var balance = await _userService.GetBalanceAsync(request.UserID);
        return Ok(balance);
    }
}

// api/v1/users/{id}/wallet