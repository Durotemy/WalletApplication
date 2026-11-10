
using WalletApi.Enums;

namespace WalletApi.Dtos;

public record LedgerRequest(
    Guid FromWalletId,
    Guid ToWalletId,
    decimal Amount,
    string Description,
    string Note
);

public record LedgerResponse(
    Guid Id,
    decimal Amount,
    string Description,
    EntryType EntryType,
    DateTime CreatedAt
);
