using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerAPI.Model
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [Required]
        [Column(TypeName ="decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        [Column(TypeName ="date")]
        public DateTime Expensedate { get; set; }

        [MaxLength(150)]
        public string? Description { get; set; }

        [Required]
        public DateTime? CreateAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
