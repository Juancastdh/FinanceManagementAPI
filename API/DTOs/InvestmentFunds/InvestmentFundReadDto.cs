namespace FinanceManagement.API.DTOs.InvestmentFunds
{
    public class InvestmentFundReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Company { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public bool Deleted { get; set; }

    }
}
