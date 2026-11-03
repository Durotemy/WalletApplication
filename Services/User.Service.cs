using Microsoft.EntityFrameworkCore;
using WalletApi.Data;
using WalletApi.Dtos;
using WalletApi.Entities;
namespace WalletApi.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(UserRequest user);
    Task<WalletResponse?> GetBalanceAsync(Guid UserID);
}
public class UserService : IUserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db)

    {
        _db = db;
    }
    public async Task<User> CreateUserAsync(UserRequest request)
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
                id = Guid.NewGuid(),
                balance = 0
            }
        };
        _db.User.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
    public async Task<WalletResponse?> GetBalanceAsync(Guid userId)
    {
        return await _db.Wallet
                .AsNoTracking()
                .Where(w => w.userId == userId)
                .Select(w => new WalletResponse(w.id, w.balance))
                .FirstOrDefaultAsync();
    }
}