using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using PlaneRental.Business.Entities;
using Core.Common.Exceptions;

namespace PlaneRental.Business.Contracts
{
    [ServiceContract]
    public interface IInventoryService
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Plane UpdatePlane(Plane Plane);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void DeletePlane(int PlaneId);

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        Plane GetPlane(int PlaneId);

        [OperationContract]
        Plane[] GetAllPlanes();

        [OperationContract]
        Plane[] GetAvailablePlanes(DateTime pickupDate, DateTime returnDate);
    }
}
