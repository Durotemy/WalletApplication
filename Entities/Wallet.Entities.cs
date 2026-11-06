namespace WalletApi.Entities;

public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; } = 0;
    public List<LedgerEntry> LedgerEntries { get; set; } = new();
}
