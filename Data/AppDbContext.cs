using Microsoft.EntityFrameworkCore;
using WalletApi.Entities;

namespace WalletApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> User => Set<User>();
    public DbSet<Wallet> Wallet => Set<Wallet>();
}

