using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletApi.Entities;

namespace WalletApi.Data.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>

{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(x => x.Note)
               .HasMaxLength(500);
        builder.Property(x => x.Status)
           .HasConversion<string>()
           .HasMaxLength(20);

        builder.HasOne(x => x.FromWallet)
        .WithMany()
        .HasForeignKey(x => x.FromWalletId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToWallet)
                .WithMany()
                .HasForeignKey(x => x.ToWalletId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.LedgerEntries)
               .WithOne(l => l.Transfer)
               .HasForeignKey(l => l.TransferId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.FromWalletId);
        builder.HasIndex(x => x.ToWalletId);
        builder.HasIndex(x => x.CreatedAt);


        builder.ToTable("Transfers", t =>
{
    t.HasCheckConstraint("CK_Transfer_Amount_Positive", "\"Amount\" > 0");
    t.HasCheckConstraint("CK_Transfer_NotSelf", "\"FromWalletId\" <> \"ToWalletId\"");
});

    }
}