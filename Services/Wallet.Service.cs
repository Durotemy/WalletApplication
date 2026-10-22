
using WalletApi.Data;
using WalletApi.Dtos;

namespace WalletApi.Services;

public interface IWalletService
{
    Task<Entities.Wallet> CreateWalletAsync(WalletRequest request);
}




public class WalletService : IWalletService
{
    private readonly AppDbContext _db;
    public WalletService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Entities.Wallet> CreateWalletAsync(WalletRequest request)
    {
        var wallet = new Entities.Wallet
        {
            id = Guid.NewGuid(),
            userId = Guid.NewGuid(),
            balance = request.balance
        };
        _db.Wallet.Add(wallet);
        await _db.SaveChangesAsync();
        return wallet;
    }
}
