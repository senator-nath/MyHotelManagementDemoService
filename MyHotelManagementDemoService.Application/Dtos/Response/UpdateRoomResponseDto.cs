using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Response
{
    public class UpdateRoomResponseDto
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateUpdated { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomAmenitiesId { get; set; }
    }
}
