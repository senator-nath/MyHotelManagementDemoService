using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.Id);


            builder.Property(r => r.RoomNumber)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(r => r.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.DateCreated)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(r => r.Url)
          .HasConversion(
              v => string.Join(',', v),
              v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            builder.HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(r => r.RoomAmenities)
                .WithOne(ra => ra.Room)
                .HasForeignKey<Room>(r => r.RoomAmenitiesId)
                .OnDelete(DeleteBehavior.Cascade);


        }


    }

}