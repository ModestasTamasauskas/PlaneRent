using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Business.Entities;
using Core.Common.Contracts;

namespace PlaneRental.Data.Contracts
{
    public interface IPlaneRepository : IDataRepository<Plane>
    {
    }
}
