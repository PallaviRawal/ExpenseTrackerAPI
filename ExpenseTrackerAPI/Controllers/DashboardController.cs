using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard-summary")]
        public async Task<IActionResult> GetDashboardSummary(
    [FromQuery] int month,
    [FromQuery] int year)
        {
            
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");

            if (year < 2000 || year > DateTime.UtcNow.Year + 1)
                return BadRequest("Invalid year.");

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            
            var totalExpense = await _context.Expenses
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .SumAsync(e => (decimal?)e.Amount) ?? 0;

            
            var currentMonthExpense = await _context.Expenses
                .Where(e => e.UserId == userId
                    && !e.IsDeleted
                    && e.Expensedate.Month == month
                    && e.Expensedate.Year == year)
                .SumAsync(e => (decimal?)e.Amount) ?? 0;

            
            var topCategoryData = await _context.Expenses
                .Where(e => e.UserId == userId
                    && !e.IsDeleted
                    && e.Expensedate.Month == month
                    && e.Expensedate.Year == year)
                .GroupBy(e => e.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .FirstOrDefaultAsync();

            var response = new DashboardSummaryDto
            {
                TotalExpense = totalExpense,
                CurrentMonthExpense = currentMonthExpense,
                TopCategory = topCategoryData?.Category ?? "N/A",
                TopCategoryExpense = topCategoryData?.Amount ?? 0
            };

            return Ok(response);
        }

    }
}
