using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyHotelManagementDemoService.Domain.Entities;

namespace HotelManagement.Persistence.Configuration
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasKey(rt => rt.Id);


            builder.Property(rt => rt.TypeName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(rt => rt.Description)
                .HasMaxLength(500);

            builder.Property(rt => rt.AccessibilityFeatures)
                .HasMaxLength(250);


        }
    }
}