using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.BookingDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.CheckInDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.CheckOutDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(b => b.NumberOfGuests)
                .IsRequired();

            builder.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }

}