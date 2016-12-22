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
    public interface IAccountService : IServiceContract
    {
        #region Async operations

        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Task<Account> GetCustomerAccountInfoAsync(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        Task UpdateCustomerAccountInfoAsync(Account account);

        #endregion
    }
}
