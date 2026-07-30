namespace FinanceManagement.API.DTOs.InvestmentFundTransactions
{
    public class InvestmentFundTransactionCreateDto
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int Type { get; set; }
        public int? InvestmentFundCategoryId { get; set; }
        public string Description { get; set; }
        public int? InvestmentFundId { get; set; }
    }
}