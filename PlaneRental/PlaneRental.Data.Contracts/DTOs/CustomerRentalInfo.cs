using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Business.Entities;

namespace PlaneRental.Data.Contracts
{
    public class CustomerRentalInfo
    {
        public Account Customer { get; set; }
        public Plane Plane { get; set; }
        public Rental Rental { get; set; }
    }
}