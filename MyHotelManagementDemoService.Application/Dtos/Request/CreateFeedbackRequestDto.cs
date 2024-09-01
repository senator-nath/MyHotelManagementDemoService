using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Request
{
    public class CreateFeedbackRequestDto
    {
        public string Comments { get; set; }
        public int Rating { get; set; }
        public string UserId { get; set; }
    }
}
