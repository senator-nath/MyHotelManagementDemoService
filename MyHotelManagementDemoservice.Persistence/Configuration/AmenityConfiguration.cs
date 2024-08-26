using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
    {

        public void Configure(EntityTypeBuilder<Amenity> builder)
        {

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(250);

            builder.Property(a => a.IsActive)
                .IsRequired();


            builder.HasOne(a => a.RoomAmenities)
                .WithMany(ra => ra.Amenities)
                .HasForeignKey(a => a.RoomAmenitiesId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}