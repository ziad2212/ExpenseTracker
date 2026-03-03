using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.DTOs.Expense
{
    public class ExpenseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public Decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public Category Category { get; set; }
    }
}
