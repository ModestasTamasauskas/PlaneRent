using System;
using System.Collections.Generic;
using System.Linq;
using PlaneRental.Business.Entities;
using Core.Common.Contracts;

namespace PlaneRental.Business.Common
{
    public interface IPlaneRentalEngine : IBusinessEngine
    {
        bool IsPlaneCurrentlyRented(int PlaneId, string accountId);
        bool IsPlaneCurrentlyRented(int PlaneId);
        bool IsPlaneAvailableForRental(int PlaneId, DateTime pickupDate, DateTime returnDate, 
                                     IEnumerable<Rental> rentedPlanes, IEnumerable<Reservation> reservedPlanes);
        Rental RentPlaneToCustomer(string loginEmail, int PlaneId, DateTime rentalDate, DateTime dateDueBack);
    }
}
