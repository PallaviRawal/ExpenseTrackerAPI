using ExpenseTrackerAPI.Controllers;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Model;
using ExpenseTrackerAPI.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Xunit;

public class DashboardControllerTests
{
    [Fact]
    public async Task GetDashboardSummary_ReturnsCorrectSummary()
    {
        // Arrange
        var context = DbContexthelper.GetDbContext();

        context.Users.Add(new User
        {
            Id = 1,
            Email = "test@test.com",
            PasswordHash = "hash"
        });

        context.Categories.Add(new Category
        {
            Id = 1,
            Name = "Food",
            UserId = 1
        });

        context.SaveChanges();

        context.Expenses.Add(new Expense
        {
            Id = 1,
            UserId = 1,
            CategoryId = 1,
            Title = "Pizza",
            Amount = 1000,
            Expensedate = new DateTime(2026, 2, 1),
            IsDeleted = false
        });


        context.SaveChanges();

        var controller = new DashboardController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }))
            }
        };

        // Act
        var result = await controller.GetDashboardSummary(2, 2026);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<DashboardSummaryDto>(okResult.Value);

        Assert.Equal(1000, data.CurrentMonthExpense);
        Assert.Equal(1000, data.TopCategoryExpense);
        Assert.Equal("Food", data.TopCategory);
    }
    [Fact]
    public async Task GetDashboardSummary_InvalidMonth_ReturnsBadRequest()
    {
        var context = DbContexthelper.GetDbContext();
        var controller = new DashboardController(context);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }))
            }
        };

        var result = await controller.GetDashboardSummary(100, 2026);

        Assert.IsType<BadRequestObjectResult>(result);
    }



}
