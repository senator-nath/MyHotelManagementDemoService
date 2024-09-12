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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Method)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>();
            builder.Property(p => p.PaymentDate)
                .IsRequired()
                .HasColumnType("datetime");

            //builder.HasOne(p => p.Booking)
            //    .WithOne(b => b.Payment)
            //    .HasForeignKey<Payment>(p => p.BookingId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
