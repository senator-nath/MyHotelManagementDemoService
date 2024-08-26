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
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Feedbacks");

            // Configure the primary key
            builder.HasKey(fb => fb.Id);

            // Configure properties
            builder.Property(fb => fb.Comments)
                .IsRequired()
                .HasMaxLength(500); // Example max length for comments

            builder.Property(fb => fb.Rating)
                .IsRequired()
                .HasDefaultValue(1) // Example default value
                .HasColumnType("int"); // Specify the data type

            builder.Property(fb => fb.DateSubmitted)
                .IsRequired()
                .HasColumnType("datetime"); // Specify the date type

            // Configure relationships

            // Many-to-One relationship with User
            builder.HasOne(fb => fb.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(fb => fb.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
