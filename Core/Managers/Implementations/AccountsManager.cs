using System.Collections.Generic;
using FinanceManagement.Core.Entities;
using FinanceManagement.Core.Exceptions;
using FinanceManagement.Core.Repositories;
using FinanceManagement.Core.UnitOfWork;

namespace FinanceManagement.Core.Managers.Implementations
{
    public class AccountsManager : IAccountsManager
    {
        private readonly IUnitOfWork UnitOfWork;

        public AccountsManager(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void AddAccount(Account account)
        {
            IRepository<Account> accountsRepository = UnitOfWork.GetRepository<Account>();
            accountsRepository.Add(account);
            UnitOfWork.SaveChanges();
        }

        public void DeleteAccountById(int id)
        {
            IRepository<Account> accountsRepository = UnitOfWork.GetRepository<Account>();
            accountsRepository.DeleteById(id);
            UnitOfWork.SaveChanges();
        }

        public Account GetAccountById(int id)
        {
            Account? account;
            IRepository<Account> accountsRepository = UnitOfWork.GetRepository<Account>();
            account = accountsRepository.GetById(id);

            if (account == null)
            {
                throw new DataNotFoundException();
            }

            return account;
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            IRepository<Account> accountsRepository = UnitOfWork.GetRepository<Account>();

            IEnumerable<Account> accounts = accountsRepository.GetAll(account => account.Deleted == false);

            return accounts;
        }

        public void UpdateAccount(Account account)
        {
            IRepository<Account> accountsRepository = UnitOfWork.GetRepository<Account>();
            accountsRepository.Update(account);
            UnitOfWork.SaveChanges();
        }
    }
}