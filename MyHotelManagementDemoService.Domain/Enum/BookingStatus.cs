using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Domain.Enum
{
    public enum BookingStatus
    {
        Reserved = 1,
        CheckedIn,
        Completed,
        Canceled
    }
}
