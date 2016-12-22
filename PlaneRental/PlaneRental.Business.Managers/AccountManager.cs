using System.ComponentModel.Composition;
using System.Security.Permissions;
using System.ServiceModel;
using Core.Common.Contracts;
using Core.Common.Exceptions;
using PlaneRental.Business.Contracts;
using PlaneRental.Business.Entities;
using PlaneRental.Common;
using PlaneRental.Data.Contracts;

namespace PlaneRental.Business.Managers
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                     ConcurrencyMode = ConcurrencyMode.Multiple,
                     ReleaseServiceInstanceOnTransactionComplete = false)]
    public class AccountManager : ManagerBase, IAccountService
    {
        public AccountManager()
        {
        }

        public AccountManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        [Import]
        IDataRepositoryFactory _DataRepositoryFactory;

        #region IAccountService operations

        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

                Account accountEntity = accountRepository.GetByLogin(loginEmail);
                if (accountEntity == null)
                {
                    NotFoundException ex = new NotFoundException(string.Format("Account with login {0} is not in database", loginEmail));
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }

                ValidateAuthorization(accountEntity);

                return accountEntity;
            });
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.PlaneRentalAdminRole)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.PlaneRentalUser)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            ExecuteFaultHandledOperation(() =>
            {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();

                ValidateAuthorization(account);
                
                Account updatedAccount = accountRepository.Update(account);
            });
        }
        
        #endregion
    }
}
