using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class PositionConfiguration: IEntityTypeConfiguration<PositionStorage>
{
    public void Configure(EntityTypeBuilder<PositionStorage> builder)
    {
        builder.ToTable("positions");

        builder.Property(p => p.Id).HasColumnName("id");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("title");
        
        builder.Property(a => a.CreateUtc)
            .HasColumnName("create_utc");

        builder.HasMany(p => p.Employees)
            .WithOne(e => e.PositionStorage)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}