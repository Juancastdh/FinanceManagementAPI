using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Managers
{
    public interface IInvestmentFundCategoriesManager
    {
        IEnumerable<InvestmentFundCategory> GetAllInvestmentFundCategories(bool? deleted = null, int? investmentFundId = null);
        void AddInvestmentFundCategory(InvestmentFundCategory investmentFundCategory);
        InvestmentFundCategory GetInvestmentFundCategoryById(int id);
        void UpdateInvestmentFundCategory(InvestmentFundCategory investmentFundCategory);
        void DeleteInvestmentFundCategoryById(int id);
    }
}
