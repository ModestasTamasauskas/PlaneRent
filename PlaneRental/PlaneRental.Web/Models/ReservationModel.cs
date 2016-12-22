using PlaneRental.Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaneRental.Web.Models
{
    public class ReservationModel
    {
        public int Plane { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
