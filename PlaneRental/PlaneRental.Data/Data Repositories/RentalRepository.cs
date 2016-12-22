using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Business.Entities;
using PlaneRental.Data.Contracts;
using Core.Common.Extensions;

namespace PlaneRental.Data
{
    [Export(typeof(IRentalRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalRepository : DataRepositoryBase<Rental>, IRentalRepository
    {
        protected override Rental AddEntity(PlaneRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override Rental UpdateEntity(PlaneRentalContext entityContext, Rental entity)
        {
            return (from e in entityContext.RentalSet
                    where e.RentalId == entity.RentalId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Rental> GetEntities(PlaneRentalContext entityContext)
        {
            return from e in entityContext.RentalSet
                   select e;
        }

        protected override Rental GetEntity(PlaneRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.RentalSet
                         where e.RentalId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<Rental> GetRentalHistoryByPlane(int PlaneId)
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.PlaneId == PlaneId
                            select e;

                return query.ToFullyLoaded();
            }
        }

        public Rental GetCurrentRentalByPlane(int PlaneId)
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.PlaneId == PlaneId && e.DateReturned == null
                            select e;

                return query.FirstOrDefault();
            }
        }

        public IEnumerable<Rental> GetCurrentlyRentedPlanes()
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.DateReturned == null
                            select e;

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(string accountId)
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from e in entityContext.RentalSet
                            where e.AccountId == accountId
                            select e;

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo()
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from r in entityContext.RentalSet
                            where r.DateReturned == null
                            //join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                            join c in entityContext.PlaneSet on r.PlaneId equals c.PlaneId
                            select new CustomerRentalInfo()
                            {
                                //Customer = a,
                                Plane = c,
                                Rental = r
                            };

                return query.ToFullyLoaded();
            }
        }
    }
}
