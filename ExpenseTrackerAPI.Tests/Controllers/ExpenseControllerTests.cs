using ExpenseTrackerAPI.Controllers;
using ExpenseTrackerAPI.DTOs;
using ExpenseTrackerAPI.Model;
using ExpenseTrackerAPI.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerAPI.Tests.Controllers
{
    public class ExpenseControllerTests
    {
        [Fact]
        public async Task DeleteExpense_ValidId_SetsIsDeletedTrue()
        {
            // Arrange
            var context = DbContexthelper.GetDbContext();

            context.Expenses.Add(new Expense
            {
                Id = 1,
                Title = "Test",
                UserId = 1,
                CategoryId = 1,
                Amount = 100,
                Expensedate = DateTime.Now,
                IsDeleted = false
            });

            context.SaveChanges();

            var controller = new ExpenseController(context);

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
            await controller.DeleteExpense(1);

            // Assert
            var expense = context.Expenses
    .IgnoreQueryFilters()
    .FirstOrDefault(e => e.Id == 1);

            Assert.NotNull(expense);
            Assert.True(expense!.IsDeleted);

        }

        [Fact]
        public async Task GetExpenses_Page1_ReturnsCorrectData()
        {
            //Arrange
            var context = DbContexthelper.GetDbContext();

            context.Users.Add(new User
            {
                Id = 1,
                Email = "test@test.com",
                PasswordHash = "hash"
            });

            // Add category
            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Food",
                UserId = 1
            });

            context.SaveChanges();

            // Add expenses
            context.Expenses.AddRange(
                new Expense { Id = 1, UserId = 1, CategoryId = 1, Title = "A", Amount = 100, Expensedate = DateTime.Now, IsDeleted = false },
                new Expense { Id = 2, UserId = 1, CategoryId = 1, Title = "B", Amount = 200, Expensedate = DateTime.Now, IsDeleted = false },
                new Expense { Id = 3, UserId = 1, CategoryId = 1, Title = "C", Amount = 300, Expensedate = DateTime.Now, IsDeleted = false }
            );

            context.SaveChanges();

            var controller = new ExpenseController(context);
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

            //Act
            var result = await controller.GetExpenses(1, 2);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PagedResponseDto<ExpenseListDto>>(okResult.Value);
            //var total = context.Expenses.IgnoreQueryFilters().Count();

            Assert.Equal(1, response.PageNumber);
            Assert.Equal(2, response.PageSize);
            Assert.Equal(3, response.TotalRecords);
            Assert.Equal(2, response.Data.Count);

        }
        [Fact]
        public async Task GetExpenses_Page2_ReturnsRemainingData()
        {
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

            context.Expenses.AddRange(
                new Expense { Id = 1, UserId = 1, CategoryId = 1, Title = "A", Amount = 100, Expensedate = DateTime.UtcNow, IsDeleted = false },
                new Expense { Id = 2, UserId = 1, CategoryId = 1, Title = "B", Amount = 200, Expensedate = DateTime.UtcNow, IsDeleted = false },
                new Expense { Id = 3, UserId = 1, CategoryId = 1, Title = "C", Amount = 300, Expensedate = DateTime.UtcNow, IsDeleted = false }
            );

            context.SaveChanges();

            var controller = new ExpenseController(context);

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

            var result = await controller.GetExpenses(2, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PagedResponseDto<ExpenseListDto>>(okResult.Value);

            Assert.Equal(1, response.Data.Count);
        }

        [Fact]
        public async Task GetExpenses_PageOutOfRange_ReturnsEmptyList()
        {
            var context = DbContexthelper.GetDbContext();

            context.Expenses.Add(new Expense
            {
                Id = 1,
                UserId = 1,
                CategoryId = 1,
                Title = "A",
                Amount = 100,
                Expensedate = DateTime.UtcNow,
                IsDeleted = false
            });

            context.SaveChanges();

            var controller = new ExpenseController(context);

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

            var result = await controller.GetExpenses(5, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PagedResponseDto<ExpenseListDto>>(okResult.Value);

            Assert.Empty(response.Data);
        }



    }
}
