using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.DTOs.Expense;
using ExpenseTrackerAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExpenseController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Add Expense
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] CreateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(
    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
);


            var expense = new Expense
            {
                Title = dto.Title.Trim(),
                Amount = dto.Amount,
                Expensedate = dto.ExpenseDate.Date,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense added successfully" });
        }

        //  2. Get Monthly Expenses
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyExpenses(int month, int year)
        {
            int userId = int.Parse(
    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
);


            var expenses = await _context.Expenses
                .Where(e =>
                    e.UserId == userId && !e.IsDeleted &&
                    e.Expensedate.Month == month &&
                    e.Expensedate.Year == year)
                .Include(e => e.Category)
                .Select(e => new ExpenseResponseDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.Expensedate,
                    CategoryName = e.Category.Name
                })
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();

            return Ok(expenses);
        }

        //  3. Update Expense
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = int.Parse(
    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
);


            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
                return NotFound("Expense not found");

            expense.Title = dto.Title.Trim();
            expense.Amount = dto.Amount;
            expense.Expensedate = dto.ExpenseDate.Date;
            expense.CategoryId = dto.categoryId;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense updated successfully" });
        }

        // 4. Delete Expense
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            int userId = int.Parse(
    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value
);
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
                return NotFound("Expense not found");

            expense.IsDeleted = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Expense deleted successfully" });
        }

        [HttpGet("monthly-summary")]
        public async Task<IActionResult> GetMonthlySummary(
    [FromQuery] int month,
    [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User not authenticated");

            int userId = int.Parse(userIdClaim.Value);

            var summary = await _context.Expenses
                .Where(e => e.UserId == userId &&
                            !e.IsDeleted &&
                            e.Expensedate.Month == month &&
                            e.Expensedate.Year == year)
                .GroupBy(e => e.Category.Name)
                .Select(g => new ExpenseSummaryDto
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            return Ok(summary);
        }


        [HttpGet]
        public async Task<IActionResult> GetExpenses(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0 || pageSize > 50) pageSize = 10;

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = _context.Expenses
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .Include(e => e.Category)
                .OrderByDescending(e => e.Expensedate);

            int totalRecords = await query.CountAsync();

            var expenses = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExpenseListDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Amount = e.Amount,
                    ExpenseDate = e.Expensedate,
                    CategoryName = e.Category.Name
                })
                .ToListAsync();

            var response = new PagedResponseDto<ExpenseListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = expenses
            };

            return Ok(response);
        }


    }
}
