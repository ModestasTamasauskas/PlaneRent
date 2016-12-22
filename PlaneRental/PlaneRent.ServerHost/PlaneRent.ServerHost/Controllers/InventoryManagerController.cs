using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Composition;
using System.Security.Permissions;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using PlaneRental.Business.Common;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using PlaneRental.Data.Contracts;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PlaneRent.ServerHost.Controllers
{
    [Route("api/[controller]/")]
    public class InventoryManagerController : Controller
    {
        public InventoryManagerController(IDataRepositoryFactory dataRepositoryFactory, IBusinessEngineFactory businessEngineFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _BusinessEngineFactory = businessEngineFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        [Import]
        IBusinessEngineFactory _BusinessEngineFactory;

        #region IInventoryService operations

        [HttpPost]
        [Route("UpdatePlane")]
        public Plane UpdatePlane([FromBody]Plane Plane)
        {
            IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();

            Plane updatedEntity = null;

            if (Plane.PlaneId == 0)
                updatedEntity = PlaneRepository.Add(Plane);
            else
                updatedEntity = PlaneRepository.Update(Plane);

            return updatedEntity;
        }

        [HttpGet]
        [Route("DeletePlane/{PlaneId}")]
        public void DeletePlane(int PlaneId)
        {

            IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();

            PlaneRepository.Remove(PlaneId);
        }

        [HttpGet]
        [Route("GetPlane")]
        public Plane GetPlane(int PlaneId)
        {
                IPlaneRepository PlaneRepository = _DataRepositoryFactory.GetDataRepository<IPlaneRepository>();

                Plane PlaneEntity = PlaneRepository.Get(PlaneId);

                return PlaneEntity;

        }

        [HttpGet]
        [Route("GetAllPlanes")]
        public Plane[] GetAllPlanes()
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

        }
        [Route("GetAvailablePlanes")]
        [HttpGet]
        public Plane[] GetAvailablePlanes(DateTime pickupDate, DateTime returnDate)
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
        }

        #endregion
    }
}
