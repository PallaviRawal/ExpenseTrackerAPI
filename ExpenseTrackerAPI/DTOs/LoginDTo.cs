using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
    public class LoginDTo
    {
        [Required , EmailAddress]
        public string ? Email { get; set; }
        [Required]
        public string ? Password { get; set; }
    }
}
