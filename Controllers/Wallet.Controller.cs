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


    // [HttpPost("fund")]
    [HttpPost("{userId:guid}/fund")]
    public async Task<IActionResult> FundWallet(Guid userId, [FromBody] FundWalletRequest request)
    {
        var fundWalletResponse = await _walletService.FundWalletAsync(userId, request.amount, request.description);
        return Ok(fundWalletResponse);
    }

}

