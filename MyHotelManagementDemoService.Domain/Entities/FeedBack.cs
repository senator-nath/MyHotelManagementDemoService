using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Domain.Entities
{
    public class FeedBack
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; } // e.g., 1 to 5 stars
        public DateTime DateSubmitted { get; set; }
 
        public int UserId { get; set; }
        public User User { get; set; }


    }
}
