using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface IInvestmentFundsManager
    {
        IEnumerable<InvestmentFund> GetAllInvestmentFunds(bool? deleted = null);
        void AddInvestmentFund(InvestmentFund investmentFund);
        InvestmentFund GetInvestmentFundById(int id);
        void UpdateInvestmentFund(InvestmentFund investmentFund);
        void DeleteInvestmentFundById(int id);
    }
}
