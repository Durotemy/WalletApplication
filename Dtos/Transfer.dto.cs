
namespace WalletApi.Dtos;

public record TransferRequest(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    string Description,
    string Note
);

public record TransferResponse(
    Guid Id,
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    string Description,
    string Note,
    DateTime CreatedAt
);