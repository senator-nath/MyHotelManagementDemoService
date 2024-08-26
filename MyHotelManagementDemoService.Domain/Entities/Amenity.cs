using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Domain.Entities
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }  // Changed to bool for simplicity

        // One-to-Many Relationship: One Amenity can be associated with many RoomAmenities
        public int RoomAmenitiesId { get; set; } // Foreign key
        public RoomAmenities RoomAmenities { get; set; }
    }
}
