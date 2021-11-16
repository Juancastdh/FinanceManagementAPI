namespace FinanceManagement.API.DTOs.FinancialTransactions
{
    public class FinancialReportReadDto
    {
        public IEnumerable<FinancialTransactionReadDto>? FinancialTransactions { get; set; }
        public decimal TotalValue { get; set; }
    }
}
