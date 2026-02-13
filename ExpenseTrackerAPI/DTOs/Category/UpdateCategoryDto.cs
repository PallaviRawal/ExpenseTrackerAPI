using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public String ? Name { get; set; }
    }
}
