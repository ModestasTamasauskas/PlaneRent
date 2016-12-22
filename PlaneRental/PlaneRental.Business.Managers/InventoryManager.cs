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
    public class InventoryManager : ManagerBase, IInventoryService
    {
        public InventoryManager()
        {
        }
        
        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        public InventoryManager(IBusinessEngineFactory businessEngineFactory)
        {
            _BusinessEngineFactory = businessEngineFactory;
        }

        public InventoryManager(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        #region IInventoryService operations

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public Plane UpdatePlane(Plane Plane)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();

                Plane updatedEntity = null;
                
                if (Plane.PlaneId == 0)
                    updatedEntity = PlaneRepository.Add(Plane);
                else
                    updatedEntity = PlaneRepository.Update(Plane);

                return updatedEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        public void DeletePlane(int PlaneId)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();

                PlaneRepository.Remove(PlaneId);
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public Plane GetPlane(int PlaneId)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();

                Plane PlaneEntity = PlaneRepository.Get(PlaneId);
                if (PlaneEntity == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Plane with ID of {0} is not in database", PlaneId));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                return PlaneEntity;
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public Plane[] GetAllPlanes()
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();

                IEnumerable<Plane> Planes = PlaneRepository.Get();
                IEnumerable<Rental> rentedPlanes = rentalRepository.GetCurrentlyRentedPlanes();

                foreach (Plane Plane in Planes)
                {
                    Rental rentedPlane = rentedPlanes.Where(item => item.PlaneId == Plane.PlaneId).FirstOrDefault();
                    Plane.CurrentlyRented = (rentedPlane != null);
                }
                
                return Planes.ToArray();
            });
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public Plane[] GetAvailablePlanes(DateTime pickupDate, DateTime returnDate)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();
                IRentalRepository rentalRepository = _DataRepositoryFactory.GetDataRepository<IRentalRepository>();
                IReservationRepository reservationRepository = _DataRepositoryFactory.GetDataRepository<IReservationRepository>();

                IPlaneRentalEngine PlaneRentalEngine = _BusinessEngineFactory.GetBusinessEngine<IPlaneRentalEngine>();

                IEnumerable<Plane> allPlanes = PlaneRepository.Get();
                IEnumerable<Rental> rentedPlanes = rentalRepository.GetCurrentlyRentedPlanes();
                IEnumerable<Reservation> reservedPlanes = reservationRepository.Get();

                List<Plane> availablePlanes = new List<Plane>();

                foreach (Plane Plane in allPlanes)
                {
                    if (PlaneRentalEngine.IsPlaneAvailableForRental(Plane.PlaneId, pickupDate, returnDate, rentedPlanes, reservedPlanes))
                        availablePlanes.Add(Plane);
                }

                return availablePlanes.ToArray();
            });
        }
        
        #endregion
    }
}
