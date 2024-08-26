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
    public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.HasKey(fb => fb.Id);


            builder.Property(fb => fb.Comments)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(fb => fb.Rating)
                .IsRequired()
                .HasDefaultValue(1)
                .HasColumnType("int");

            builder.Property(fb => fb.DateSubmitted)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne(fb => fb.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
