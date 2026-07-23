using System;
using System.Collections.Generic;

namespace FinanceManagement.Core.Entities
{
    public class InvestmentFundTransaction: BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public InvestmentFundTransactionType Type { get; set; }
        public InvestmentFundCategory? InvestmentFundCategory { get; set; }
        public int? InvestmentFundCategoryId { get; set; }
        public string Description { get; set; }
        public InvestmentFund? InvestmentFund { get; set; }
        public int? InvestmentFundId { get; set; }

    }
}
