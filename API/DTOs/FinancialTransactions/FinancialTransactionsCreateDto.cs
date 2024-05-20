namespace FinanceManagement.API.DTOs.FinancialTransactions
{
    public class FinancialTransactionsCreateDto
    {
        public IEnumerable<FinancialTransactionCreateDto> FinancialTransactions { get; set; }
    }
}
