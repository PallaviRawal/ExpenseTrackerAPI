namespace ExpenseTrackerAPI.DTOs
{
    public class ExpenseListDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string? CategoryName { get; set; }
    }
}
