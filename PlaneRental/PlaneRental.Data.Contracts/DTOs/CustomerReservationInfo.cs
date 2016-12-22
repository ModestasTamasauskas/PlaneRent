using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Business.Entities;

namespace PlaneRental.Data.Contracts
{
    public class CustomerReservationInfo
    {
        public Account Customer { get; set; }
        public Plane Plane { get; set; }
        public Reservation Reservation { get; set; }
    }
}
