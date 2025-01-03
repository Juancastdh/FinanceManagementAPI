

namespace FinanceManagement.Core.Entities
{
    public class Account : BaseEntity
    {

        public required string Identifier { get; set; }

        public required string Description { get; set; }

    }

}