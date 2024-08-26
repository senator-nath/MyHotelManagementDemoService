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
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReferenceDetails)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(r => r.DateIssued)
                .IsRequired()
                .HasColumnType("datetime");
        }
    }
}
