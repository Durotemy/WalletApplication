using Microsoft.AspNetCore.Mvc;
using WalletApi.Dtos;
using WalletApi.Services;


namespace WalletApi.Controllers;

[ApiController]
[Route("api/v1/transfers")]

public class TransferController : ControllerBase

{
    private readonly ITransferService _transferService;

    public TransferController(ITransferService transferService)

    {
        _transferService = transferService;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransferRequest transferRequest)
    {
        var transfer = await _transferService.CreateTransfer(transferRequest);
        return Created($"api/v1/transfers/{transfer.Id}", transfer);
    }
}
