using System.ComponentModel.DataAnnotations;
namespace ExpenseTrackerAPI.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(255)]
        public String ? Name { get; set; } = string.Empty;
    }
}
