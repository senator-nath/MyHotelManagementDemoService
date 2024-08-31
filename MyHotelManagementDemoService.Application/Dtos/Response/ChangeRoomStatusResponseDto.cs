using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Response
{
    public class ChangeRoomStatusResponseDto
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
    }
}
