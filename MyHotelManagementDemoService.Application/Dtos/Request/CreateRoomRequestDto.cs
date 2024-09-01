using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Request
{
    public class CreateRoomRequestDto
    {
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
}
