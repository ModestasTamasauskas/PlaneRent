using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using PlaneRental.Client.Contracts;
using PlaneRental.Client.Entities;
using PlaneRental.Web.Core;
using PlaneRental.Web.Models;
using Core.Common.Contracts;

namespace PlaneRental.Web.Controllers.API
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [RoutePrefix("api/reservation")]
    [UsesDisposableService]
    public class ReservationApiController : ApiControllerBase
    {
        [ImportingConstructor]
        public ReservationApiController(IInventoryService inventoryService, IRentalService rentalService)
        {
            _InventoryService = inventoryService;
            _RentalService = rentalService;
        }

        IInventoryService _InventoryService;
        IRentalService _RentalService;

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(_InventoryService);
            disposableServices.Add(_RentalService);
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("availableplanes")]
        public HttpResponseMessage GetAvailablePlanes(HttpRequestMessage request, DateTime pickupDate, DateTime returnDate)
        {

            Plane[] planes =  _InventoryService.GetAvailablePlanesAsync(pickupDate, returnDate).Result;

            return request.CreateResponse<Plane[]>(HttpStatusCode.OK, planes);

        }

        [HttpPost]
        [Route("reserveplane")]
        public HttpResponseMessage ReservePlane(HttpRequestMessage request, [FromBody]ReservationModel reservationModel)
        {

            HttpResponseMessage response = null;

            string user = UserName(); // this method is secure to only the authenticated user to reserve
            Reservation reservation = _RentalService.MakeReservationAsync(user, reservationModel.Plane, reservationModel.PickupDate, reservationModel.ReturnDate).Result;

            response = request.CreateResponse<Reservation>(HttpStatusCode.OK, reservation);

            return response;


        }

        [HttpGet]
        [Route("getopen")]
        public HttpResponseMessage GetOpenReservations(HttpRequestMessage request)
        {
            
            HttpResponseMessage response = null;

            string user = UserName(); // this method is secure to only the authenticated user to reserve
            CustomerReservationData[] reservations = _RentalService.GetCustomerReservationsAsync(user).Result;

            response = request.CreateResponse<CustomerReservationData[]>(HttpStatusCode.OK, reservations);

            return response;

        }

        [HttpGet]
        [Route("cancel")]
        public HttpResponseMessage CancelReservation(HttpRequestMessage request, int reservationId)
        {

            HttpResponseMessage response = null;

            // not that calling the WCF service here will authenticate access to the data
            Reservation reservation = _RentalService.GetReservationAsync(reservationId);
            if (reservation != null)
            {
                _RentalService.CancelReservationAsync(reservationId).Wait();

                response = request.CreateResponse(HttpStatusCode.OK);
            }
            else
                response = request.CreateErrorResponse(HttpStatusCode.NotFound, "No reservation found under that ID.");

            return response;

        }
    }
}
