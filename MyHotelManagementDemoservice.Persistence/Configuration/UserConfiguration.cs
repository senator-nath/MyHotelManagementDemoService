using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Users");

            // Configure the primary key
            builder.HasKey(u => u.Id);

            // Configure properties
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50); // Example max length

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50); // Example max length

            builder.Property(u => u.AgeGroup)
                .HasMaxLength(20); // Example max length

            builder.Property(u => u.Address)
                .HasMaxLength(200); // Example max length

            builder.Property(u => u.StateId)
                .IsRequired()
                .HasMaxLength(50); // Example max length



            // Configure relationships
            builder.HasOne(u => u.State)
                .WithMany()
                .HasForeignKey(u => u.StateId);

        }

    }
}