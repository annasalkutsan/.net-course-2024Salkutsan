using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("currencies");

        builder.Property(c => c.Id)
            .HasColumnName("id");
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10) 
            .HasColumnName("code");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100) 
            .HasColumnName("name");

        builder.Property(a => a.CreateUtc)
            .HasColumnName("create_utc");

        builder.HasMany(c => c.Accounts)
            .WithOne(a => a.Currency)
            .HasForeignKey(a => a.CurrencyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}