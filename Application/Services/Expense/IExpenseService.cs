using ExpenseTracker.Application.DTOs.Expense;

namespace ExpenseTracker.Application.Services.Expense
{
    public interface IExpenseService
    {
        Task<List<ExpenseResponse>> GetExpenses(Guid userId, string? filter, DateTime? start, DateTime? end);
        Task<ExpenseResponse> GetExpenseById(Guid userId, Guid expenseId);
        Task<ExpenseResponse> CreateExpense(Guid userId, CreateExpenseRequest request);
        Task<ExpenseResponse> UpdateExpense(Guid userId, Guid expenseId, UpdateExpenseRequest request);
        Task<bool> DeleteExpense(Guid userId, Guid expenseId);
    }
}
