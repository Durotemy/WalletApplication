using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Entities;
using System.Text.Json;
namespace WalletApi.Services;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(UserRequest user);

    Task<User?> GetUserByIdAsync(Guid userId);

    Task<List<UserResponse>> GetAllUser();

    Task<WalletResponse?> GetBalanceAsync(Guid UserID);
}
public class UserService : IUserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db)

    {
        _db = db;
    }
    public async Task<UserResponse> CreateUserAsync(UserRequest request)
    {
        var user = new User
        {
            UserName = request.Username,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Name = request.Name,
            Wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 0
            }
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        var userResponse = new UserResponse(user.Id, user.Name, user.Email, user.UserName, user.Wallet == null ? null : new WalletResponse(user.Wallet.Id, user.Wallet.Balance));
        // return new List<UserResponse> { userResponse };
        return userResponse;
    }
    public async Task<WalletResponse?> GetBalanceAsync(Guid userId)
    {
        var user = await _db.Users
            .Include(u => u.Wallet)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        Console.WriteLine($"UserId: {userId}");

        if (user == null)
        {
            Console.WriteLine("User not found.");
            return null;
        }

        if (user.Wallet == null)
        {
            Console.WriteLine("User has no wallet.");
            return null;
        }

        Console.WriteLine($"Wallet ID: {user.Wallet.Id}");
        Console.WriteLine($"Wallet Balance: {user.Wallet.Balance}");

        return new WalletResponse(
            user.Wallet.Id,
            user.Wallet.Balance
        );
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await _db.Users.Include(u => u.Wallet).FirstOrDefaultAsync(u => u.Id == userId);
        Console.WriteLine($"User: {JsonSerializer.Serialize(user)}"); if (user == null)
        {
            Console.WriteLine("User not found.");
            return null;
        }
        Console.WriteLine($"GetUserByIdAsync: UserId: {userId}, User: {user?.UserName}");
        return user;
    }

    public async Task<List<UserResponse>> GetAllUser()
    {
        var user = await _db.Users.Include(u => u.Wallet).ThenInclude(w => w!.LedgerEntries).ToListAsync();
        return await _db.Users
            .AsNoTracking()
            .Select(u => new UserResponse(
                u.Id, u.Name, u.Email, u.UserName,
                u.Wallet == null ? null : new WalletResponse(
                    u.Wallet.Id,
                    u.Wallet.Balance,
                    u.Wallet.LedgerEntries
                        .OrderByDescending(l => l.CreatedAt)
                        .Select(l => new LedgerResponse(l.Id, l.Amount, l.Description, l.EntryType, l.CreatedAt))
                        .ToList())))
            .ToListAsync();

        // return user;
    }

}