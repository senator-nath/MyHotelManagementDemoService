using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Domain.Entities
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AgeGroup { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public State State { get; set; }



        public ICollection<Booking> Bookings { get; set; }
        public List<FeedBack> Feedbacks { get; set; }
    }
}
