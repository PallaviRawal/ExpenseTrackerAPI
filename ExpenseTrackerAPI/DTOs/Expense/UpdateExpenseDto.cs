using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.Expense
{
    public class UpdateExpenseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = null;
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }
        [Required]
        public int categoryId { get; set; }

    }
}
