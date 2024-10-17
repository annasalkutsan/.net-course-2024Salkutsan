using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.Property(a => a.Id)
            .HasColumnName("id");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasColumnName("amount");

        builder.Property(a => a.CreateUtc)
            .HasColumnName("create_utc");

        builder.HasOne(a => a.Client)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Currency)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CurrencyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}