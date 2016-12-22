using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaneRental.Common
{
    public class PlaneCurrentlyRentedException : ApplicationException
    {
        public PlaneCurrentlyRentedException(string message)
            : base(message)
        {
        }

        public PlaneCurrentlyRentedException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
