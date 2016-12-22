using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Business.Entities;
using PlaneRental.Data.Contracts;
using Core.Common.Extensions;

namespace PlaneRental.Data
{
    [Export(typeof(IPlaneRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaneRepository : DataRepositoryBase<Plane>, IPlaneRepository
    {
        protected override Plane AddEntity(PlaneRentalContext entityContext, Plane entity)
        {
            return entityContext.PlaneSet.Add(entity);
        }

        protected override Plane UpdateEntity(PlaneRentalContext entityContext, Plane entity)
        {
            return (from e in entityContext.PlaneSet
                    where e.PlaneId == entity.PlaneId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Plane> GetEntities(PlaneRentalContext entityContext)
        {
            return from e in entityContext.PlaneSet
                   select e;
        }

        protected override Plane GetEntity(PlaneRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.PlaneSet
                         where e.PlaneId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }
    }
}
