using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.Expense
{
    public class CreateExpenseDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required]
        [Range(0.01, 99999999.99)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
