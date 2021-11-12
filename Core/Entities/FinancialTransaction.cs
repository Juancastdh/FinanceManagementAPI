using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Entities
{
    public class FinancialTransaction: BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public bool IsExpense { get; set; }
    }
}
