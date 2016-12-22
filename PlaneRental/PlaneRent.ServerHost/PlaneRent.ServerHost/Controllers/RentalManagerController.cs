using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using PlaneRental.Business.Common;
using PlaneRental.Business.Contracts;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using PlaneRental.Data.Contracts;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PlaneRent.ServerHost.Controllers
{
    [Route("api/[controller]")]
    public class RentalManagerController : Controller
    {

        public RentalManagerController(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        #region IRentalService operations

        [Route("RentPlaneToCustomer")]
        [HttpPost]
        public Rental RentPlaneToCustomer(string loginEmail, int PlaneId, DateTime dateDueBack)
        {
           
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                Rental rental = PlaneRentalEngine.RentPlaneToCustomer(loginEmail, PlaneId, DateTime.Now, dateDueBack);

                return rental;   
        }

        [Route("RentPlaneToCustomer")]
        [HttpPost]
        public Rental RentPlaneToCustomer(string loginEmail, int PlaneId, DateTime rentalDate, DateTime dateDueBack)
        {
            IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

            Rental rental = PlaneRentalEngine.RentPlaneToCustomer(loginEmail, PlaneId, rentalDate, dateDueBack);

            return rental;
                
        }

        [Route("AcceptPlaneReturn")]
        [HttpPost]
        public void AcceptPlaneReturn(int PlaneId)
        {
            
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                Rental rental = rentalRepository.GetCurrentRentalByPlane(PlaneId);

                rental.DateReturned = DateTime.Now;

                Rental updatedRentalEntity = rentalRepository.Update(rental);

        }

        [Route("GetRentalHistory")]
        [HttpGet]
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
           
                //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                //Account account = accountRepository.GetByLogin(loginEmail);

                IEnumerable<Rental> rentalHistory = rentalRepository.GetRentalHistoryByAccount(loginEmail);

                return rentalHistory;

        }

        [Route("GetReservation")]
        //[HttpPost]
        [HttpGet]
        public Reservation GetReservation(int reservationId)
        {

                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);

                return reservation;

        }

        [Route("MakeReservation")]
        [HttpPost]
        public Reservation MakeReservation(string loginEmail, int PlaneId, DateTime rentalDate, DateTime returnDate)
        {
                //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                //Account account = accountRepository.GetByLogin(loginEmail);

                Reservation reservation = new Reservation()
                {
                    AccountId = loginEmail,
                    PlaneId = PlaneId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                Reservation savedEntity = reservationRepository.Add(reservation);

                return savedEntity;
        }

        [Route("ExecuteRentalFromReservation")]
        [HttpPost]
        public void ExecuteRentalFromReservation(int reservationId)
        {

                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                Reservation reservation = reservationRepository.Get(reservationId);
                

                //Account account = accountRepository.Get(reservation.AccountId);
                


                Rental rental = PlaneRentalEngine.RentPlaneToCustomer(reservation.AccountId, reservation.PlaneId, reservation.RentalDate, reservation.ReturnDate);

               
        }

        [Route("CancelReservation")]
        [HttpGet]
        public void CancelReservation(int reservationId)
        {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);

                reservationRepository.Remove(reservationId);
        }

        [Route("GetCurrentReservations")]
        [HttpGet]
        public CustomerReservationData[] GetCurrentReservations()
        {

            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

            IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCurrentCustomerReservationInfo();
            foreach (CustomerReservationInfo reservationInfo in reservationInfoSet)
            {
                reservationData.Add(new CustomerReservationData()
                {
                    ReservationId = reservationInfo.Reservation.ReservationId,
                    Plane = reservationInfo.Plane.Color + " " + reservationInfo.Plane.Year + " " + reservationInfo.Plane.Description,
                    CustomerName = reservationInfo.Reservation.AccountId,
                    RentalDate = reservationInfo.Reservation.RentalDate,
                    ReturnDate = reservationInfo.Reservation.ReturnDate
                });
            }

            return reservationData.ToArray();

        }

        [Route("GetCustomerReservations")]
        [HttpGet]
        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {

           // IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

            //Account account = accountRepository.GetByLogin(loginEmail);

            List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

            IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(loginEmail);
            foreach (CustomerReservationInfo reservationInfo in reservationInfoSet)
            {
                reservationData.Add(new CustomerReservationData()
                {
                    ReservationId = reservationInfo.Reservation.ReservationId,
                    Plane = reservationInfo.Plane.Color + " " + reservationInfo.Plane.Year + " " + reservationInfo.Plane.Description,
                    CustomerName = reservationInfo.Reservation.AccountId,
                    RentalDate = reservationInfo.Reservation.RentalDate,
                    ReturnDate = reservationInfo.Reservation.ReturnDate
                });
            }

            return reservationData.ToArray();

        }

        [Route("GetRental")]
        [HttpGet]
        public Rental GetRental(int rentalId)
        {

                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                Rental rental = rentalRepository.Get(rentalId);


                return rental;

        }

        [Route("GetCurrentRentals")]
        [HttpGet]
        public CustomerRentalData[] GetCurrentRentals()
        {
            
            IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

            List<CustomerRentalData> rentalData = new List<CustomerRentalData>();

            IEnumerable<CustomerRentalInfo> rentalInfoSet = rentalRepository.GetCurrentCustomerRentalInfo();
            foreach (CustomerRentalInfo rentalInfo in rentalInfoSet)
            {
                rentalData.Add(new CustomerRentalData()
                {
                    RentalId = rentalInfo.Rental.RentalId,
                    Plane = rentalInfo.Plane.Color + " " + rentalInfo.Plane.Year + " " + rentalInfo.Plane.Description,
                    CustomerName = rentalInfo.Rental.AccountId,
                    DateRented = rentalInfo.Rental.DateRented,
                    ExpectedReturn = rentalInfo.Rental.DateDue
                });
            }

            return rentalData.ToArray();
        }

        [Route("GetDeadReservations")]
        [HttpGet]
        public Reservation[] GetDeadReservations()
        {
           
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                IEnumerable<Reservation> reservations = reservationRepository.GetReservationsByPickupDate(DateTime.Now.AddDays(-1));

                return (reservations != null ? reservations.ToArray() : null);
        }

        [Route("IsPlaneCurrentlyRented")]
        [HttpPost]
        public bool IsPlaneCurrentlyRented(int PlaneId)
        {
            
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                return PlaneRentalEngine.IsPlaneCurrentlyRented(PlaneId);
        }

        #endregion
    }
}
