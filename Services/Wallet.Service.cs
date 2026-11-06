using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Entities;
namespace WalletApi.Services;

public interface IWalletService
{
    Task<FundWalletResponse?> FundWalletAsync(Guid userId, decimal amount);
}

public class WalletService : IWalletService
{
    private readonly AppDbContext _db;
    public WalletService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<FundWalletResponse?> FundWalletAsync(Guid userId, decimal amount)
    {
        var wallet = _db.Wallets.FirstOrDefault(w => w.UserId == userId);
        var user = _db.Users.FirstOrDefault(w => w.Id == userId);
        var transactionId = Guid.NewGuid().ToString();
        if (wallet is null) return null;

        if (user is null) return null;


        wallet.Balance += amount;
        await _db.SaveChangesAsync();
        return new FundWalletResponse(wallet.Id, wallet.Balance, transactionId, "success", "funded successfully", user.UserName);
    }
}
