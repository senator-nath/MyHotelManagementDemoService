using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Domain.Entities
{
    public class RoomAmenities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }  // Changed to bool for simplicity

        // One-to-One Relationship: One RoomAmenities can be associated with one Room
        public Room Room { get; set; }  // Navigation property to Room

        // One-to-Many Relationship: One RoomAmenities can have many Amenities
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
