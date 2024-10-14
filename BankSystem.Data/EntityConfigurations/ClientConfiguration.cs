using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankSystem.Data.EntityConfigurations;

public class ClientConfiguration: IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("first_name");

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("last_name");

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnName("phone_number");

        builder.Property(c => c.BirthDay)
            .IsRequired()
            .HasColumnName("birth_day");

        builder.Property(c => c.Passport)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("passport");

        builder.Property(c => c.CreateUtc)
            .HasColumnName("create_utc");

        builder.HasMany(c => c.Accounts)
            .WithOne(a => a.Client)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}