using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagement.Core.Entities
{
    public class InvestmentFundCategory : BaseEntity
    {
        public string? Name { get; set; }
        public bool Deleted { get; set; }
        public InvestmentFund? InvestmentFund { get; set; }
        public int InvestmentFundId { get; set; }
        public ICollection<InvestmentFundTransaction>? InvestmentFundTransactions { get; set; }
    }
}