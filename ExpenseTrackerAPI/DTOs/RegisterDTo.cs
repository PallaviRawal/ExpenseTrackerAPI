using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Model
{
    public class RegisterDTo
    {
        [Required]
        public string? FullName { get; set; }
        [Required,EmailAddress]
        public String? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required,MinLength(6)]
        public String? PasswordHash { get; set; }
    }
}
