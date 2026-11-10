
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUser();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        var userResponse = new UserResponse(user.Id, user.Name, user.Email, user.UserName, user.Wallet == null ? null : new WalletResponse(user.Wallet.Id, user.Wallet.Balance));
        return Ok(userResponse);
    }

    [HttpGet("{id}/wallet")]
    public async Task<IActionResult> GetBalace([FromRoute] UserBalanceRequest request)
    {
        var balance = await _userService.GetBalanceAsync(request.UserID);
        return Ok(balance);
    }
}

// api/v1/users/{id}/wallet