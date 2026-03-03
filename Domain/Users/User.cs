using ExpenseTracker.Domain.Expenses;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Domain.Users
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 12)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
