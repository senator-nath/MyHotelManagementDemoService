using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Response
{
    public class CreateFeedbackResponseDto
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string UserId { get; set; }
    }
}
