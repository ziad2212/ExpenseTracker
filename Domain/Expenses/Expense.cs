using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ExpenseTracker.Domain.Expenses
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
       
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]

        public decimal Amount { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
      
        public DateTime Date { get; set; }
        [Required]
       
        public Category Category { get; set; }

    }
}
