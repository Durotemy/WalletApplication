using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Enums;
using WalletApi.Entities;
namespace WalletApi.Services;


public interface IWalletService
{
    Task<FundWalletResponse?> FundWalletAsync(Guid userId, decimal amount, string description);
}

public class WalletService : IWalletService
{
    private readonly AppDbContext _db;
    public WalletService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<FundWalletResponse?> FundWalletAsync(Guid userId, decimal amount, string description)
    {
        if (amount <= 0) return null;

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) return null;

        var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet is null) return null;

        await using var tx = await _db.Database.BeginTransactionAsync();

        await _db.Wallets
            .Where(w => w.Id == wallet.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(w => w.Balance, w => w.Balance + amount));

        var ledgerEntry = new LedgerEntry
        {
            Id = Guid.NewGuid(),
            WalletId = wallet.Id,
            Amount = amount,
            EntryType = EntryType.Credit,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
        _db.LedgerEntries.Add(ledgerEntry);
        await _db.SaveChangesAsync();

        await tx.CommitAsync();

        var newBalance = await _db.Wallets
            .Where(w => w.Id == wallet.Id)
            .Select(w => w.Balance)
            .FirstAsync();

        return new FundWalletResponse(wallet.Id, newBalance, ledgerEntry.Description, ledgerEntry.Id.ToString(),
            "success", "funded successfully", user.UserName);
    }
}
