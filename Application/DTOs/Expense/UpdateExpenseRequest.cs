using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.DTOs.Expense
{
    public class UpdateExpenseRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public Decimal? Amount { get; set; }

        public Category? Category { get; set; }
    }
}
