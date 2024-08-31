using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Response
{
    public class GetRefundsResponseDto
    {
        public int Id { get; set; }
        public string ReferenceDetails { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateIssued { get; set; }
    }
}
