using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MyHotelManagementDemoService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Persistence.Configuration
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Refunds");

            // Configure the primary key
            builder.HasKey(r => r.Id);

            // Configure properties
            builder.Property(r => r.ReferenceDetails)
                .IsRequired()
                .HasMaxLength(100); // Example max length

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // Specify decimal precision

            builder.Property(r => r.DateIssued)
                .IsRequired()
                .HasColumnType("datetime"); // Specify the date type
        }
    }
}
