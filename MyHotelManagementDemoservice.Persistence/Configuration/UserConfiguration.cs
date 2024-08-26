using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);


            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.AgeGroup)
                .HasMaxLength(20);

            builder.Property(u => u.Address)
                .HasMaxLength(200);

            builder.Property(u => u.StateId)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(u => u.State)
                .WithMany()
                .HasForeignKey(u => u.StateId);

        }

    }
}