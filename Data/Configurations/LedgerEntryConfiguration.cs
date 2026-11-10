using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletApi.Entities;


namespace WalletApi.Data.Configurations;

public class LedgerEntryConfiguration : IEntityTypeConfiguration<LedgerEntry>
{
       public void Configure(EntityTypeBuilder<LedgerEntry> builder)
       {
              //  builder.ToTable("LedgerEntries");
              builder.ToTable("LedgerEntries", t =>
                 t.HasCheckConstraint("CK_LedgerEntry_Amount_Positive", "\"Amount\" > 0"));

              builder.HasKey(x => x.Id);

              builder.Property(x => x.Id)
                     .ValueGeneratedOnAdd();

              builder.Property(x => x.Amount)
                     .HasPrecision(18, 2)
                     .IsRequired();

              builder.Property(x => x.EntryType)
              .IsRequired()
              .HasConversion<string>()
              .HasMaxLength(20)
              .IsRequired(); ;

              builder.Property(x => x.Description)
                     .HasMaxLength(500)
                     .IsRequired();
       }
}