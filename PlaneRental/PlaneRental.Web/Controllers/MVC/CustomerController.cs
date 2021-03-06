﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using PlaneRental.Web.Core;

namespace PlaneRental.Web.Controllers.MVC
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
    [RoutePrefix("customer")]
    public class CustomerController : ViewControllerBase
    {
        /*public ActionResult Index()
        {
            return View();
        }*/

        [HttpGet]
        [Route("account")]
        public ActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        [Route("reserve")]
        public ActionResult ReservePlane()
        {
            return View();
        }

        [HttpGet]
        [Route("reservations")]
        public ActionResult CurrentReservations()
        {
            return View();
        }

        [HttpGet]
        [Route("history")]
        public ActionResult RentalHistory()
        {
            return View();
        }
    }
}
