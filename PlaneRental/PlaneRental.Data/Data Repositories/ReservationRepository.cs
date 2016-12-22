using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Business.Entities;
using PlaneRental.Data.Contracts;
using Core.Common.Extensions;

namespace PlaneRental.Data
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReservationRepository : DataRepositoryBase<Reservation>, IReservationRepository
    {
        protected override Reservation AddEntity(PlaneRentalContext entityContext, Reservation entity)
        {
            return entityContext.ReservationSet.Add(entity);
        }

        protected override Reservation UpdateEntity(PlaneRentalContext entityContext, Reservation entity)
        {
            return (from e in entityContext.ReservationSet
                    where e.ReservationId == entity.ReservationId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Reservation> GetEntities(PlaneRentalContext entityContext)
        {
            return from e in entityContext.ReservationSet
                   select e;
        }

        protected override Reservation GetEntity(PlaneRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.ReservationSet
                         where e.ReservationId == id
                         select e);

            var results = query.FirstOrDefault();

            return results;
        }

        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            //join a in entityContext.AccountSet on r.AccountId equals a.FirstName
                            join c in entityContext.PlaneSet on r.PlaneId equals c.PlaneId
                            select new CustomerReservationInfo()
                            {
                                //Customer = a,
                                Plane = c,
                                Reservation = r
                            };

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate)
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            where r.RentalDate < pickupDate
                            select r;

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(string accountId)
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                var query = from r in entityContext.ReservationSet
                            //join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                            join c in entityContext.PlaneSet on r.PlaneId equals c.PlaneId
                            where r.AccountId == accountId
                            select new CustomerReservationInfo()
                            {
                                //Customer = a,
                                Plane = c,
                                Reservation = r
                            };

                return query.ToFullyLoaded();
            }
        }
    }
}
