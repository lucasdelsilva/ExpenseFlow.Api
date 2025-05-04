using ExpenseFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Infrastructure.DataAccess;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>().ToTable("Tags");
    }
}