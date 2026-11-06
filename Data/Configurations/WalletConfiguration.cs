using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletApi.Entities;

namespace WalletApi.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Balance)
               .HasPrecision(18, 2)
               .IsRequired();


        builder.HasOne<User>()
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(w => w.LedgerEntries)
            .WithOne(l => l.Wallet)
            .HasForeignKey(l => l.WalletId)
       .OnDelete(DeleteBehavior.Restrict);

    }
}


