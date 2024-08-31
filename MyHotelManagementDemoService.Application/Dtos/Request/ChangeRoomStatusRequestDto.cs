using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Request
{
    public class ChangeRoomStatusRequestDto
    {
        public int RoomId { get; set; }
        public string NewStatus { get; set; }
    }
}
