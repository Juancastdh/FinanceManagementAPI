using FinanceManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Models
{
    public class FinancialReport
    {
        public IEnumerable<FinancialTransaction>? FinancialTransactions { get; set; }
        public decimal TotalValue { get; set; }
    }
}
