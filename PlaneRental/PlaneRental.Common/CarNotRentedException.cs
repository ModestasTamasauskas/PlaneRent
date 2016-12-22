using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaneRental.Common
{
    public class PlaneNotRentedException : ApplicationException
    {
        public PlaneNotRentedException(string message)
            : base(message)
        {
        }

        public PlaneNotRentedException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
