using System;

namespace FinanceManagement.Core.Entities
{
    public class InvestmentFund: BaseEntity
    {
        public string? Name { get; set; }
        public string? Company { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
    }
}
