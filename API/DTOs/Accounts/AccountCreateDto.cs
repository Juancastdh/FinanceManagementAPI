namespace FinanceManagement.API.DTOs.Accounts
{
    public class AccountCreateDto
    {
        public required string Identifier { get; set; }
        public required string Description { get; set; }
    }
}