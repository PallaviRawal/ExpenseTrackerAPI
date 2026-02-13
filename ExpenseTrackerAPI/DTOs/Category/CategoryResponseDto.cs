
namespace ExpenseTrackerAPI.DTOs.Category
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
