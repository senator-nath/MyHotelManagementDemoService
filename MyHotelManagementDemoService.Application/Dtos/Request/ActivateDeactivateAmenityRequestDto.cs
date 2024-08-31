using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Dtos.Request
{
    public class ActivateDeactivateAmenityRequestDto
    {
        public int AmenityId { get; set; }
        public bool IsActive { get; set; }
    }
}
