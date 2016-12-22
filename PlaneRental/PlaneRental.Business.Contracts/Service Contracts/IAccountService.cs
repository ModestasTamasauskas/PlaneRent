using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using Core.Common.Exceptions;

namespace PlaneRental.Business.Contracts
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        [FaultContract(typeof(NotFoundException))]
        [FaultContract(typeof(AuthorizationValidationException))]
        Account GetCustomerAccountInfo(string loginEmail);

        [OperationContract]
        [FaultContract(typeof(AuthorizationValidationException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void UpdateCustomerAccountInfo(Account account);
    }
}
