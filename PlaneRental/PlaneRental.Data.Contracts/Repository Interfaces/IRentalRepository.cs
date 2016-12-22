using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Business.Entities;
using Core.Common.Contracts;

namespace PlaneRental.Data.Contracts
{
    public interface IRentalRepository : IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByPlane(int PlaneId);
        Rental GetCurrentRentalByPlane(int PlaneId);
        IEnumerable<Rental> GetCurrentlyRentedPlanes();
        IEnumerable<Rental> GetRentalHistoryByAccount(string accountId);
        IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo();
    }
}
