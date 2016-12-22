using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [RoutePrefix("api/customer")]
    [UsesDisposableService]
    public class CustomerApiController : ApiControllerBase
    {
        [ImportingConstructor]
        public CustomerApiController(IAccountService accountService)
        {
            _AccountService = accountService;
        }

        IAccountService _AccountService;

        protected override void RegisterServices(List<IServiceContract> disposableServices)
        {
            disposableServices.Add(_AccountService);
        }

        [HttpGet]
        [Route("account")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetCustomerAccountInfo(HttpRequestMessage request)
        {

            HttpResponseMessage response = null;

            Account account = await _AccountService.GetCustomerAccountInfoAsync(UserName());
            // notice no need to create a seperate model object since Account entity will do just fine

            response = request.CreateResponse<Account>(HttpStatusCode.OK, account);

            return response;

        }

        [HttpPost]
        [Route("account")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> UpdateCustomerAccountInfo(HttpRequestMessage request, Account accountModel)
        {

            HttpResponseMessage response = null;

            ValidateAuthorizedUser(accountModel.LoginEmail);

            List<string> errors = new List<string>();

            List<State> states = UIHelper.GetStates();
            State state = states.Where(item => item.Abbrev.ToUpper() == accountModel.State.ToUpper()).FirstOrDefault();
            if (state == null)
                errors.Add("Invalid state.");

            // trim out the / in the exp date
            accountModel.ExpDate = accountModel.ExpDate.Substring(0, 2) + accountModel.ExpDate.Substring(3, 2);

            if (errors.Count == 0)
            {
                await _AccountService.UpdateCustomerAccountInfoAsync(accountModel);
                response = request.CreateResponse(HttpStatusCode.OK);
            }
            else
                response = request.CreateResponse<string[]>(HttpStatusCode.BadRequest, errors.ToArray());

            return response;

        }
    }
}
