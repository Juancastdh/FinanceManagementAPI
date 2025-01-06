﻿namespace FinanceManagement.API.DTOs.FinancialTransactions
{
    public class FinancialTransactionReadDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsExpense { get; set; }
        public int PeriodId { get; set; }
        public int AccountId { get; set; }
    }
}
