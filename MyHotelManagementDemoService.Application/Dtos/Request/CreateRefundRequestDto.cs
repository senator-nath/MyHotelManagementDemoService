using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Request
{
    public class CreateRefundRequestDto
    {
        public string ReferenceDetails { get; set; }
        public decimal Amount { get; set; }
    }
}
