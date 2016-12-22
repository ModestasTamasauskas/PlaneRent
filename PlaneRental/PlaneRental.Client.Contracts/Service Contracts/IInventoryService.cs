using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PlaneRental.Client.Entities;
using Core.Common.Contracts;
using Core.Common.Exceptions;

namespace PlaneRental.Client.Contracts
{
    [ServiceContract]
    public interface IInventoryService : IServiceContract
    {
        #region Async operations

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task<Plane> UpdatePlaneAsync(Plane Plane);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task DeletePlaneAsync(int PlaneId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Task<Plane> GetPlaneAsync(int PlaneId);

        [OperationContract]
        Task<Plane[]> GetAllPlanesAsync();

        [OperationContract]
        Task<Plane[]> GetAvailablePlanesAsync(DateTime pickupDate, DateTime returnDate);

        #endregion
    }
}
