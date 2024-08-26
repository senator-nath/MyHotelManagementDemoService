using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {

        public void Configure(EntityTypeBuilder<Amenity> builder)
        {
            // Specify the table name (optional, defaults to the class name)
            builder.ToTable("Amenities");

            // Configure the primary key
            builder.HasKey(a => a.Id);

            // Configure properties
            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100); // Example max length

            builder.Property(a => a.Description)
                .HasMaxLength(250); // Example max length

            builder.Property(a => a.IsActive)
                .IsRequired(); // Bool type does not need max length

            // Configure relationships
            builder.HasOne(a => a.RoomAmenities)
                .WithMany(ra => ra.Amenities)
                .HasForeignKey(a => a.RoomAmenitiesId)
                .OnDelete(DeleteBehavior.Cascade); // Example cascade delete behavior

        }
    }
}