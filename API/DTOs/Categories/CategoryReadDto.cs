namespace FinanceManagement.API.DTOs.Categories
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Percentage { get; set; }
        public bool Deleted { get; set; }

    }
}
