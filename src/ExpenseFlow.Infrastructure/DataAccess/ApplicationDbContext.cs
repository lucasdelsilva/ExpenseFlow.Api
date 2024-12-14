using ExpenseFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Infrastructure.DataAccess;
internal class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
}