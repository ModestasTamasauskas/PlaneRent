using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using PlaneRental.Business.Contracts;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using PlaneRental.Data.Contracts;
using System.ComponentModel.Composition;

namespace PlaneRent.ServerHost.Controllers
{
    [Route("api/[controller]")]

    public class AccountManagerController : Controller
    {

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        #region IAccountService operations

        //[PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        //[PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        [HttpPost("GetCustomerAccountInfo")]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

            Account accountEntity = accountRepository.GetByLogin(loginEmail);
            

            return accountEntity;
        }

        [HttpPost]
        [HttpPost("UpdateCustomerAccountInfo")]
        public void UpdateCustomerAccountInfo(Account account)
        {
           
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                Account updatedAccount = accountRepository.Update(account);
        }
        #endregion
    }
}
