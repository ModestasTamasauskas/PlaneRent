using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using PlaneRental.Business.Common;
using PlaneRental.Business.Contracts;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using PlaneRental.Data.Contracts;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using System.Security.Permissions;

namespace PlaneRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     ReleaseServiceInstanceOnTransactionComplete = false)]
    public class RentalManager : ManagerBase, IRentalService
    {
        public RentalManager()
        {
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public RentalManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public RentalManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        protected override Account LoadAuthorizationValidationAccount(string loginName)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account authAcct = accountRepository.GetByLogin(loginName);
            if (authAcct == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("Cannot find account for login name {0} to use for security trimming.", loginName));
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }

            return authAcct;
        }
        
        #region IRentalService operations

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public Rental RentPlaneToCustomer(string loginEmail, int PlaneId, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                try
                {
                    Rental rental = PlaneRentalEngine.RentPlaneToCustomer(loginEmail, PlaneId, DateTime.Now, dateDueBack);

                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (PlaneCurrentlyRentedException ex)
                {
                    throw new FaultException<PlaneCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public Rental RentPlaneToCustomer(string loginEmail, int PlaneId, DateTime rentalDate, DateTime dateDueBack)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                try
                {
                    Rental rental = PlaneRentalEngine.RentPlaneToCustomer(loginEmail, PlaneId, rentalDate, dateDueBack);

                    return rental;
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (PlaneCurrentlyRentedException ex)
                {
                    throw new FaultException<PlaneCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
            });
        }
        
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public void AcceptPlaneReturn(int PlaneId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                Rental rental = rentalRepository.GetCurrentRentalByPlane(PlaneId);
                if (rental == null)
                {
                    PlaneNotRentedException ex = new PlaneNotRentedException(string.Format("Plane {0} is not currently rented.", PlaneId));
                    throw new FaultException<PlaneNotRentedException>(ex, ex.Message);
                }

                rental.DateReturned = DateTime.Now;

                Rental updatedRentalEntity = rentalRepository.Update(rental);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public IEnumerable<Rental> GetRentalHistory(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                /*Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);*/

                IEnumerable<Rental> rentalHistory = rentalRepository.GetRentalHistoryByAccount(loginEmail);

                return rentalHistory;
            });
        }

        /*[PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]*/
        public Reservation GetReservation(int reservationId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No reservation found for id '{0}'.", reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                //ValidateAuthorization(reservation);

                return reservation;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public Reservation MakeReservation(string loginEmail, int PlaneId, DateTime rentalDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                /*Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);*/

                Reservation reservation = new Reservation()
                {
                    AccountId = loginEmail,
                    PlaneId = PlaneId,
                    RentalDate = rentalDate,
                    ReturnDate = returnDate
                };

                Reservation savedEntity = reservationRepository.Add(reservation);

                return savedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public void ExecuteRentalFromReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Reservation {0} is not found.", reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                /*Account account = accountRepository.Get(reservation.AccountId);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for account ID '{0}'.", reservation.AccountId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }*/

                try
                {
                    Rental rental = PlaneRentalEngine.RentPlaneToCustomer(reservation.AccountId, reservation.PlaneId, reservation.RentalDate, reservation.ReturnDate);
                }
                catch (UnableToRentForDateException ex)
                {
                    throw new FaultException<UnableToRentForDateException>(ex, ex.Message);
                }
                catch (PlaneCurrentlyRentedException ex)
                {
                    throw new FaultException<PlaneCurrentlyRentedException>(ex, ex.Message);
                }
                catch (NotFoundException ex)
                {
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                reservationRepository.Remove(reservation);
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public void CancelReservation(int reservationId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                Reservation reservation = reservationRepository.Get(reservationId);
                if (reservation == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No reservation found found for ID '{0}'.", reservationId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                //ValidateAuthorization(reservation);

                reservationRepository.Remove(reservationId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public CustomerReservationData[] GetCurrentReservations()
        {
            return ExecuteFaultHandledOperation(() =>
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
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public CustomerReservationData[] GetCustomerReservations(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                //IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

               /* Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No account found for login '{0}'.", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(account);*/

                List<CustomerReservationData> reservationData = new List<CustomerReservationData>();

                IEnumerable<CustomerReservationInfo> reservationInfoSet = reservationRepository.GetCustomerOpenReservationInfo(loginEmail);
                foreach (CustomerReservationInfo reservationInfo in reservationInfoSet)
                {
                    reservationData.Add(new CustomerReservationData()
                    {
                        ReservationId = reservationInfo.Reservation.ReservationId,
                        Plane = reservationInfo.Plane.Color + " " + reservationInfo.Plane.Year + " " + reservationInfo.Plane.Description,
                        CustomerName = reservationInfo.Customer.FirstName + " " + reservationInfo.Customer.LastName,
                        RentalDate = reservationInfo.Reservation.RentalDate,
                        ReturnDate = reservationInfo.Reservation.ReturnDate
                    });
                }

                return reservationData.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public Rental GetRental(int rentalId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                Rental rental = rentalRepository.Get(rentalId);
                if (rental == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("No rental record found for id '{0}'.", rentalId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                //ValidateAuthorization(rental);

                return rental;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public CustomerRentalData[] GetCurrentRentals()
        {
            return ExecuteFaultHandledOperation(() =>
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
                        CustomerName = rentalInfo.Customer.FirstName + " " + rentalInfo.Customer.LastName,
                        DateRented = rentalInfo.Rental.DateRented,
                        ExpectedReturn = rentalInfo.Rental.DateDue
                    });
                }

                return rentalData.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public Reservation[] GetDeadReservations()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                IEnumerable<Reservation> reservations = reservationRepository.GetReservationsByPickupDate(DateTime.Now.AddDays(-1));

                return (reservations != null ? reservations.ToArray() : null);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public bool IsPlaneCurrentlyRented(int PlaneId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                return PlaneRentalEngine.IsPlaneCurrentlyRented(PlaneId);
            });
        }

        #endregion
    }
}
