namespace FinanceManagement.API.DTOs.FinancialTransactions
{
    public class FinancialTransactionCreateDto
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsExpense { get; set; }
        public int PeriodId { get; set; }

    }
}
