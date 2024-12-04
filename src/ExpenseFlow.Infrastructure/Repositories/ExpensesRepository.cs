using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Infrastructure.DataAccess;

namespace ExpenseFlow.Infrastructure.Repositories;
internal class ExpensesRepository : IExpensesRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ExpensesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
    }
}