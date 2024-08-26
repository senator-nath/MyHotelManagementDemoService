﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }  // Changed to DateTime for correct data type
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        // One-to-One Relationship: Each Room can have one RoomAmenities
        public int RoomAmenitiesId { get; set; }  // Foreign key for the one-to-one relationship
        public RoomAmenities RoomAmenities { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
