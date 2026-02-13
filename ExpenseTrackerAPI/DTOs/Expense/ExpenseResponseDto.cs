namespace ExpenseTrackerAPI.DTOs.Expense
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
