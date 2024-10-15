﻿using BankSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BankSystem.Data.EntityConfigurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.Property(e => e.Id).HasColumnName("id"); 
        builder.HasKey(e => e.Id); 

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("first_name");

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("last_name");

        builder.Property(e => e.PhoneNumber)
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

        builder.Property(e => e.Contract)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("contract");

        builder.Property(e => e.Salary)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("salary"); 

        builder.Property(e => e.PositionId)
            .HasColumnName("position_id");

        builder.Property(a => a.CreateUtc)
            .HasColumnName("create_utc");

        builder.HasOne(e => e.PositionStorage)
            .WithMany(p => p.Employees)
            .HasForeignKey(e => e.PositionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}