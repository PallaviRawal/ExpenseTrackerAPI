namespace ExpenseTrackerAPI.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public String? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public String? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
