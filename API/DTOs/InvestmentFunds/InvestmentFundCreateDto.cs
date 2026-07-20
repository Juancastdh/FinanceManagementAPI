namespace FinanceManagement.API.DTOs.InvestmentFunds
{
    public class InvestmentFundCreateDto
    {
        public string? Name { get; set; }
        public string? Company { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
    }
}
