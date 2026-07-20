using FinanceManagement.API.DTOs.InvestmentFunds;

namespace FinanceManagement.API.DTOs.InvestmentFundCategories
{
    public class InvestmentFundCategoryReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Deleted { get; set; }
        public int InvestmentFundId { get; set; }
        public InvestmentFundReadDto? InvestmentFund { get; set; }

    }
}
