using WalletApi.Entities;
using Microsoft.EntityFrameworkCore;
using WalletApi.Dtos;
using WalletApi.Data;
namespace WalletApi.Services;

public interface ITransferService
{
    Task<TransferResponse> CreateTransfer(TransferRequest Transfer);
}
public class TransferService : ITransferService
{
    private readonly AppDbContext _db;
    public TransferService(AppDbContext db)

    {
        _db = db;
    }

    public async Task<TransferResponse> CreateTransfer(TransferRequest Transfer)
    {
        if (Transfer.Amount <= 0)
        {
            throw new Exception("Transfer amount must be greater than zero.");
        }
        var receiveId = Transfer.ToWalletId;
        var senderRId = Transfer.FromWalletId;

        var receiverWallet = await _db.Wallets
            .FirstOrDefaultAsync(w => w.Id == receiveId);

        var senderWallet = await _db.Wallets
        .FirstOrDefaultAsync(w => w.Id == senderRId);

        if (receiverWallet == null)
        {
            throw new Exception("Receiver wallet not found.");
        }

        if (senderWallet == null)
        {
            throw new Exception("Sender wallet not found.");
        }

        if (senderWallet.Id == receiverWallet.Id)
        {
            throw new Exception("Sender and receiver wallets cannot be the same.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync();

        var rows = await _db.Wallets
            .Where(w => w.Id == senderWallet.Id && w.Balance >= Transfer.Amount)
            .ExecuteUpdateAsync(s => s.SetProperty(w => w.Balance, w => w.Balance - Transfer.Amount));
        if (rows == 0) throw new Exception("Insufficient balance.");

        await _db.Wallets
            .Where(w => w.Id == receiverWallet.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(w => w.Balance, w => w.Balance + Transfer.Amount));

        var transferLedger = new Transfer
        {
            Id = Guid.NewGuid(),
            FromWalletId = senderWallet.Id,
            ToWalletId = receiverWallet.Id,
            Amount = Transfer.Amount,
            Note = Transfer.Note,
            Status = Enums.StatusTypes.Completed,
            CreatedAt = DateTime.UtcNow,

            LedgerEntries =
            {
                new LedgerEntry
                {
                    Id = Guid.NewGuid(),
                    WalletId = senderWallet.Id,
                    Amount = Transfer.Amount,
                    EntryType = Enums.EntryType.Debit,
                    Description = $"Transfer to wallet {receiverWallet.Id}",
                    CreatedAt = DateTime.UtcNow
                },
                new LedgerEntry
                {
                    Id = Guid.NewGuid(),
                    WalletId = receiverWallet.Id,
                    Amount = Transfer.Amount,
                    EntryType = Enums.EntryType.Credit,
                    Description = $"Transfer from wallet {senderWallet.Id}",
                    CreatedAt = DateTime.UtcNow
                }
            }
        };

        _db.Transfers.Add(transferLedger);
        await _db.SaveChangesAsync();

        await tx.CommitAsync();

        return new TransferResponse(
            transferLedger.Id,
            transferLedger.FromWalletId,
            transferLedger.ToWalletId,
            transferLedger.Amount,
            "Transfer completed",
            transferLedger.Note ?? "",
            transferLedger.CreatedAt);
    }

}