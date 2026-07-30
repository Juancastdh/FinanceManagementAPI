using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface IInvestmentFundTransactionsManager
    {
        IEnumerable<InvestmentFundTransaction> GetAllInvestmentFundTransactions(int? investmentFundCategoryId = null, int? investmentFundId = null);
        void AddInvestmentFundTransaction(InvestmentFundTransaction investmentFundTransaction);
        InvestmentFundTransaction GetInvestmentFundTransactionById(int id);
        void UpdateInvestmentFundTransaction(InvestmentFundTransaction investmentFundTransaction);
        void DeleteInvestmentFundTransactionById(int id);
        decimal GetSumOfInvestmentFundTransactionValuesByInvestmentFundCategoryId(int investmentFundCategoryId);
    }
}
