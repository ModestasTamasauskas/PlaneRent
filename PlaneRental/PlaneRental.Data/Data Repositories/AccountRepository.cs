using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using PlaneRental.Business.Entities;
using PlaneRental.Data.Contracts;

namespace PlaneRental.Data
{    
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : DataRepositoryBase<Account>, IAccountRepository
    {
        protected override Account AddEntity(PlaneRentalContext entityContext, Account entity)
        {
            return entityContext.AccountSet.Add(entity);
        }

        protected override Account UpdateEntity(PlaneRentalContext entityContext, Account entity)
        {
            return (from e in entityContext.AccountSet
                    where e.AccountId == entity.AccountId
                    select e).FirstOrDefault();
        }

        protected override IEnumerable<Account> GetEntities(PlaneRentalContext entityContext)
        {
            return from e in entityContext.AccountSet
                   select e;
        }
        
        protected override Account GetEntity(PlaneRentalContext entityContext, int id)
        {
            var query = (from e in entityContext.AccountSet
                         where e.AccountId == id
                         select e);
            
            var results = query.FirstOrDefault();

            return results;
        }

        public Account GetByLogin(string login)
        {
            using (PlaneRentalContext entityContext = new PlaneRentalContext())
            {
                return (from a in entityContext.AccountSet
                        where a.LoginEmail == login
                        select a).FirstOrDefault();
            }
        }
    }
}
