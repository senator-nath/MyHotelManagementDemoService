using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelManagement.Persistence.Configuration
{public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.RoomNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Price)
            .HasColumnType("decimal(18,2)");

            builder.Property(r => r.RoomAmenityId)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(r => r.RoomTypeId)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(r => r.status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(r => r.RoomType)
            .WithMany(rt => rt.Rooms)
            .HasForeignKey(r => r.RoomTypeId);

        // builder.HasMany(r => r.Bookings)
        //     .WithOne(b => b.Room)
        //     .HasForeignKey(b => b.RoomId);

        builder.HasOne(r => r.RoomAmenity)
            .WithOne(r => r.Room)
            .HasForeignKey<Room>(r => r.RoomAmenityId);
    }

        
    }

}