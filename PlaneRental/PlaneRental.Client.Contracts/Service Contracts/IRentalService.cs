using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PlaneRental.Client.Entities;
using PlaneRental.Common;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace PlaneRental.Client.Contracts
{
    [ServiceContract]
    public interface IRentalService : IServiceContract
    {
        #region Async operations

        [OperationContract(Name = "RentPlaneToCustomerImmediatelyAsync")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(PlaneCurrentlyRentedException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<Rental> RentPlaneToCustomerAsync(string loginEmail, int PlaneId, DateTime dateDueBack);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(PlaneCurrentlyRentedException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<Rental> RentPlaneToCustomerAsync(string loginEmail, int PlaneId, DateTime rentalDate, DateTime dateDueBack);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task AcceptPlaneReturnAsync(int PlaneId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<IEnumerable<Rental>> GetRentalHistoryAsync(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Reservation GetReservationAsync(int reservationId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task<Reservation> MakeReservationAsync(string loginEmail, int PlaneId, DateTime rentalDate, DateTime returnDate);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task ExecuteRentalFromReservationAsync(int reservationId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task CancelReservationAsync(int reservationId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<CustomerReservationData[]> GetCurrentReservationsAsync();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<CustomerReservationData[]> GetCustomerReservationsAsync(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<Rental> GetRentalAsync(int rentalId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<CustomerRentalData[]> GetCurrentRentalsAsync();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<Reservation[]> GetDeadReservationsAsync();

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<bool> IsPlaneCurrentlyRentedAsync(int PlaneId);

        #endregion
    }
}
