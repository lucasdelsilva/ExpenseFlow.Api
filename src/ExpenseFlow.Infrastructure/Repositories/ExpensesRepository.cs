using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Infrastructure.Repositories;
internal class ExpensesRepository : IExpensesWriteOnlyRepository, IExpensesReadOnlyRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ExpensesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<Expense> GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id.Equals(id));
    }

    public async Task<Expense> UpdateOrRemoveGetById(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id.Equals(id));
    }

    public async Task<bool> Delete(long id)
    {
        var expense = await UpdateOrRemoveGetById(id);
        if (expense is null)
            return false;

        _dbContext.Expenses.Remove(expense);
        return true;
    }

    public void Update(Expense request)
    {
        _dbContext.Expenses.Update(request);
    }
}