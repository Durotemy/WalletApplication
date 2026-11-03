using System.ComponentModel.DataAnnotations;

namespace WalletApi.Dtos;

public record WalletRequest(
    [Required] decimal balance
);

public record WalletResponse(Guid Id, decimal Balance);


public record FundWalletRequest(
    decimal Balance
);

public record FundWalletResponse(
    Guid Id,
    decimal Balance,
    string TransactionId,
    string Status,
    string Message,
    string userName
);


