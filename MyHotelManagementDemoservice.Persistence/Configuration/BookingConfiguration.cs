using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Bookings");

            // Configure the primary key
            builder.HasKey(b => b.Id);

            // Configure properties
            builder.Property(b => b.BookingDate)
                .IsRequired()
                .HasColumnType("datetime"); // Specify the date type

            builder.Property(b => b.CheckInDate)
                .IsRequired()
                .HasColumnType("datetime"); // Specify the date type

            builder.Property(b => b.CheckOutDate)
                .IsRequired()
                .HasColumnType("datetime"); // Specify the date type

            builder.Property(b => b.NumberOfGuests)
                .IsRequired();

            builder.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(50); // Example max length

            // Configure relationships

            // One-to-One relationship with Payment
            builder.HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade); // Example cascade delete behavior

            // One-to-Many relationship with User
            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Example restrict delete behavior

            // One-to-Many relationship with Room
            builder.HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

        }


    }

}