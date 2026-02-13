namespace ExpenseTrackerAPI.DTOs
{
    public class DashboardSummaryDto
    {
        public decimal TotalExpense { get; set; }
        public decimal CurrentMonthExpense { get; set; }
        public string ? TopCategory { get; set; }
        public decimal TopCategoryExpense { get; set; }
    }
}
