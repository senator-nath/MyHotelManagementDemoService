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
        public Room Room { get; set; }
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
