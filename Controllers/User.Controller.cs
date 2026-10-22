
// using Microsoft.AspNetCore.Mvc;
// using WalletApi.Dtos;
// using WalletApi.Services;

// namespace WalletApi.Controllers;

// [ApiController]
// [Route("api/v1/wallets")]
// public class WalletController : ControllerBase
// {
//     private readonly IWalletService _walletService;

//     public WalletController(IWalletService walletService)
//     {
//         _walletService = walletService;
//     }

//     [HttpPost("login")]
//     public IActionResult GetAll() => Ok();
// }

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
}