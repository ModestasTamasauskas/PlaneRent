using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Business.Common;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using PlaneRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace PlaneRental.Business
{
    [Export(typeof(IPlaneRentalEngine))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class PlaneRentalEngine : IPlaneRentalEngine
    {
        [ImportingConstructor]
        public PlaneRentalEngine(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        IDataRepositoryFactory _DataRepositoryFactory;

        public bool IsPlaneCurrentlyRented(int PlaneId, string accountId)
        {
            bool rented = false;

            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByPlane(PlaneId);
            if (currentRental != null && currentRental.AccountId == accountId)
                rented = true;

            return rented;
        }

        public bool IsPlaneCurrentlyRented(int PlaneId)
        {
            bool rented = false;

            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            var currentRental = rentalRepository.GetCurrentRentalByPlane(PlaneId);
            if (currentRental != null)
                rented = true;

            return rented;
        }

        public bool IsPlaneAvailableForRental(int PlaneId, DateTime pickupDate, DateTime returnDate, 
                                            IEnumerable<Rental> rentedPlanes, IEnumerable<Reservation> reservedPlanes)
        {
            bool available = true;
            
            Reservation reservation = reservedPlanes.Where(item => item.PlaneId == PlaneId).FirstOrDefault();
            if (reservation != null && (
                (pickupDate >= reservation.RentalDate && pickupDate <= reservation.ReturnDate) ||
                (returnDate >= reservation.RentalDate && returnDate <= reservation.ReturnDate)))
            {
                available = false;
            }

            if (available)
            {
                Rental rental = rentedPlanes.Where(item => item.PlaneId == PlaneId).FirstOrDefault();
                if (rental != null && (pickupDate <= rental.DateDue))
                    available = false;
            }

            return available;
        }

        public Rental RentPlaneToCustomer(string loginEmail, int PlaneId, DateTime rentalDate, DateTime dateDueBack)
        {
            if (rentalDate > DateTime.Now)
                throw new UnableToRentForDateException(string.Format("Cannot rent for date {0} yet.", rentalDate.ToShortDateString()));

            //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            bool PlaneIsRented = IsPlaneCurrentlyRented(PlaneId);
            if (PlaneIsRented)
                throw new PlaneCurrentlyRentedException(string.Format("Plane {0} is already rented.", PlaneId));

            //Account account = accountRepository.GetByLogin(loginEmail);
           // if (account == null)
                //throw new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));

            Rental rental = new Rental()
            {
                AccountId = loginEmail,
                PlaneId = PlaneId,
                DateRented = rentalDate,
                DateDue = dateDueBack
            };

            Rental savedEntity = rentalRepository.Add(rental);

            return savedEntity;
        }
    }
}
