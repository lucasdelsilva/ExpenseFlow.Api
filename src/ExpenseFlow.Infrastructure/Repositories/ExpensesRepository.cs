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

    public async Task<Expense?> GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id.Equals(id));
    }

    public async Task<Expense?> UpdateOrRemoveGetById(long id)
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

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(order => order.Date)
            .ThenBy(then => then.Title)
            .ToListAsync();
    }
}