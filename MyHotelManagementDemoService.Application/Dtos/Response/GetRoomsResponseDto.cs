using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Response
{
    public class GetRoomsResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }  // Added for RoomType details
        public int RoomAmenitiesId { get; set; }
        public string RoomAmenitiesName { get; set; }  // Added for RoomAmenities details
        public List<string> Urls { get; set; }
    }
}
