﻿using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(), 
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(e => e.BirthDay)
            .IsRequired()
            .HasColumnName("birth_day")
            .HasConversion(dateTimeConverter);

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