using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Entities;
namespace WalletApi.Services;

public interface IWalletService
{
    Task<Wallet> CreateWalletAsync(WalletRequest request);
    Task<FundWalletResponse?> FundWalletAsync(Guid userId, decimal amount);
}

public class WalletService : IWalletService
{
    private readonly AppDbContext _db;
    public WalletService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Wallet> CreateWalletAsync(WalletRequest request)
    {
        var wallet = new Wallet
        {
            id = Guid.NewGuid(),
            userId = Guid.NewGuid(),
            balance = request.balance
        };
        _db.Wallet.Add(wallet);
        await _db.SaveChangesAsync();
        return wallet;
    }

    public async Task<FundWalletResponse?> FundWalletAsync(Guid userId, decimal amount)
    {
        var wallet = _db.Wallet.FirstOrDefault(w => w.userId == userId);
        var user = _db.User.FirstOrDefault(w => w.Id == userId);
        if (wallet is null) return null;

        if (user is null) return null;


        wallet.balance += amount;
        await _db.SaveChangesAsync();
        return new FundWalletResponse(wallet.id, wallet.balance, "3232323", "success", "funded successfully", user.UserName);
    }
}
