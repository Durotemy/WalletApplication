using WalletApi.Enums;
namespace WalletApi.Entities;

public class Transfer
{
    public Guid Id { get; set; }
    public Guid FromWalletId { get; set; }
    public Wallet FromWallet { get; set; } = null!;
    public Guid ToWalletId { get; set; }
    public Wallet ToWallet { get; set; } = null!;
    public string? Note { get; set; }

    public StatusTypes Status { get; set; } = Enums.StatusTypes.Pending;

    public decimal Amount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<LedgerEntry> LedgerEntries { get; set; } = new();
}
