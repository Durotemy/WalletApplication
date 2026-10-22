namespace WalletApi.Entities;

public class Wallet
{
    public Guid id { get; set; }

    public Guid userId { get; set; }
    public required decimal balance { get; set; } = 0;

}
