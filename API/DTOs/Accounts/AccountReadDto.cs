namespace FinanceManagement.API.DTOs.Accounts
{
    public class AccountReadDto
    {
        public int Id { get; set; }
        public required string Identifier { get; set; }
        public required string Description { get; set; }
    }
}