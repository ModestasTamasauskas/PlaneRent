using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PlaneRental.Web.Core;
using PlaneRental.Web.Models;
using WebMatrix.WebData;

namespace PlaneRental.Web.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [RoutePrefix("account")]
    public class AccountController : ViewControllerBase
    {
        [ImportingConstructor]
        public AccountController(ISecurityAdapter securityAdapter)
        {
            _SecurityAdapter = securityAdapter;
        }

        ISecurityAdapter _SecurityAdapter;

        [HttpGet]
        [Route("register")]
        public ActionResult Register()
        {
            _SecurityAdapter.Initialize();
            
            return View();
        }

        [HttpGet]
        [Route("login")]
        [Authorize]
        public ActionResult Login(string returnUrl)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("changepassword")]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        
        /*[HttpGet]
        [Route("forgotpassword")]
        [Authorize]
        public ActionResult ForgotPassword()
        {
            return View();
        }*/
    }
}
