using WalletApi.Enums;
namespace WalletApi.Entities;


public class LedgerEntry
{
    public Guid? TransferId { get; set; }
    public Transfer? Transfer { get; set; }
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = null!;
    public decimal Amount { get; set; }
    public EntryType EntryType { get; set; }
    public string Description { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}