using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Client.Entities;

namespace PlaneRental.Admin.Support
{
    public class PlaneEventArgs : EventArgs
    {
        public PlaneEventArgs(Plane plane, bool isNew)
        {
            Plane = plane;
            IsNew = isNew;
        }

        public Plane Plane { get; set; }
        public bool IsNew { get; set; }
    }
}
