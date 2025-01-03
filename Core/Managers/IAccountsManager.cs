using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface IAccountsManager
    {
        IEnumerable<Account> GetAllAccounts();
        void AddAccount(Account account);
        Account GetAccountById(int id);
        void UpdateAccount(Account account);
        void DeleteAccountById(int id);
    }
}
