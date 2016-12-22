using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Business.Entities;
using Core.Common.Contracts;

namespace PlaneRental.Data.Contracts
{
    public interface IReservationRepository : IDataRepository<Reservation>
    {
        IEnumerable<Reservation> GetReservationsByPickupDate(DateTime pickupDate);
        IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo();
        IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(string accountId);
    }
}
