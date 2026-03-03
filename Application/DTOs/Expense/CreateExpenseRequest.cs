using ExpenseTracker.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Application.DTOs.Expense
{
    public class CreateExpenseRequest
    {
     

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public Decimal  Amount { get; set; }

        [Required]
        public Category Category { get; set; }



        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
