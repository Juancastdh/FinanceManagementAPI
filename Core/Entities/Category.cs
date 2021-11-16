using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Entities
{
    public class Category: BaseEntity
    {
        public string? Name { get; set; }
        public int Percentage { get; set; }
        public ICollection<FinancialTransaction>? FinancialTransactions { get; set; }
    }
}
