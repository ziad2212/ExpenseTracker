using ExpenseTracker.Domain.Expenses;
using ExpenseTracker.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
       public DbSet<User> Users { get; set; }  
        public DbSet<Expense> Expenses { get; set; }
        
    }
}
