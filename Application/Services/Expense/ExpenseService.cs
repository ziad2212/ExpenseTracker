using Azure.Core;
using ExpenseTracker.Application.DTOs.Expense;
using ExpenseTracker.Domain.Users;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExpenseTracker.Application.Services.Expense
{

    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ExpenseService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ExpenseResponse> CreateExpense(Guid userId, CreateExpenseRequest request)
        {
            
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                var expense = new Domain.Expenses.Expense
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description,
                    Amount = request.Amount,
                    Category = request.Category,
                    Date = request.Date,
                    UserId = userId
                };
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return new ExpenseResponse
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category,
                    Date = expense.Date
                };


            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new ApplicationException("An error occurred while creating the expense.", ex);

            }
        }

        public async Task<bool> DeleteExpense(Guid userId, Guid expenseId)
        {
            try { 
                var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);
                if (expense == null)
                {
                    throw new KeyNotFoundException("Expense not found or does not belong to the user.");
                }
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw; // let it bubble up as-is
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the expense.", ex);
            }

        }

        public async Task<ExpenseResponse> GetExpenseById(Guid userId, Guid expenseId)
        {
            if (expenseId == Guid.Empty) throw new ArgumentNullException(nameof(expenseId));
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));

            try
            {
                var expense = await _context.Expenses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);

                if (expense == null)
                {
                    throw new KeyNotFoundException("Expense not found.");
                }

                return new ExpenseResponse
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category,
                    Date = expense.Date
                };
            }
            catch (KeyNotFoundException)
            {
                throw; // let it bubble up as-is
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred.", ex);
            }
        }

        public async Task<List<ExpenseResponse>> GetExpenses(Guid userId, string? filter, DateTime? start, DateTime? end)
        {
            try
            {
                var query = _context.Expenses.Where(e => e.UserId == userId);
                
                query = filter switch
                {
                    "past_week" => query.Where(e => e.Date >= DateTime.UtcNow.AddDays(-7)),
                    "past_month" => query.Where(e => e.Date >= DateTime.UtcNow.AddDays(-30)),
                    "last_3_months" => query.Where(e => e.Date >= DateTime.UtcNow.AddDays(-90)),
                    "custom" => query.Where(e => e.Date >= start && e.Date <= end),
                    _ => query // no filter, return all
                };
                var expenses = await query.ToListAsync();
                return expenses.Select(e => new ExpenseResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                }).ToList();

            }
            catch(Exception ex)
            {
                  throw new ApplicationException("An error occurred while retrieving expenses.", ex);
            }

        }

        public async Task<ExpenseResponse> UpdateExpense(Guid userId, Guid expenseId, UpdateExpenseRequest request)
        {
            try
            {
               
                var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == expenseId && e.UserId == userId);
                if (expense == null) {
                    throw new KeyNotFoundException(
                        "Expense not found or does not belong to the user."
                    );

                }
                expense.Name = request.Name ?? expense.Name;
                expense.Description = request.Description ?? expense.Description;
                expense.Amount = request.Amount ?? expense.Amount;
                expense.Category = request.Category ?? expense.Category;

                await _context.SaveChangesAsync();


                return new ExpenseResponse
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    Category = expense.Category,
                    Date = expense.Date
                };

            }
            catch (KeyNotFoundException)
            {
                throw; // let it bubble up as-is
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the expense.", ex);

            }
        }
    }
}
