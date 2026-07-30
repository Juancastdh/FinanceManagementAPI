using FinanceManagement.API.DTOs.InvestmentFundCategories;
using FinanceManagement.API.DTOs.InvestmentFunds;

namespace FinanceManagement.API.DTOs.InvestmentFundTransactions
{
    public class InvestmentFundTransactionReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int Type { get; set; }
        public int? InvestmentFundCategoryId { get; set; }
        public string Description { get; set; }
        public int? InvestmentFundId { get; set; }
        public InvestmentFundReadDto? InvestmentFund { get; set; }

        public InvestmentFundCategoryReadDto? InvestmentFundCategory { get; set; }

    }
}
