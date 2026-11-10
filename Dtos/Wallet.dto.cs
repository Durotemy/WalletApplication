using System.ComponentModel.DataAnnotations;

namespace WalletApi.Dtos;

public record WalletRequest(
    [Required] decimal balance
);

public record WalletResponse(Guid Id, decimal Balance, List<LedgerResponse>? LedgerEntries = null);


public record FundWalletRequest(
    [Required] decimal amount,
    [Required, StringLength(100, MinimumLength = 3)] string description

);

public record FundWalletResponse(
    Guid Id,
    decimal Balance,
    string Description,
    string TransactionId,
    string Status,
    string Message,
    string userName,
    List<LedgerResponse>? LedgerEntries = null

);


