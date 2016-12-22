using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Contracts;
using Core.Common.Data;

namespace PlaneRental.Data
{
    public abstract class DataRepositoryBase<T> : DataRepositoryBase<T, PlaneRentalContext>
        where T : class, IIdentifiableEntity, new()
    {
    }
}
