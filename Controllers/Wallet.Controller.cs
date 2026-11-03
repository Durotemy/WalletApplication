using Microsoft.AspNetCore.Mvc;
using WalletApi.Dtos;
using WalletApi.Services;

namespace WalletApi.Controllers;

[ApiController]
[Route("api/v1/wallets")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WalletRequest request)
    {
        var wallet = await _walletService.CreateWalletAsync(request);
        return Created($"api/v1/wallets/{wallet}", wallet);
    }

    [HttpPost("fund")]
    public async Task<IActionResult> FundWallet([FromBody] FundWalletRequest request)
    {
        var fundWalletResponse = await _walletService.FundWalletAsync(request);
        return Ok(fundWalletResponse);
    }

}

