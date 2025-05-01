using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

    public async Task<List<Expense>> GetAll(User user)
    {
        return await GetFullExpense().AsNoTracking().Where(expense => expense.UserId.Equals(user.Id)).ToListAsync();
    }

    public async Task<Expense?> GetById(User user, long id)
    {
        return await GetFullExpense().FirstOrDefaultAsync(expense => expense.Id.Equals(id) && expense.UserId.Equals(user.Id));
    }

    public async Task<Expense?> UpdateOrRemoveGetById(User user, long id)
    {
        return await GetFullExpense().FirstOrDefaultAsync(expense => expense.Id.Equals(id) && expense.UserId.Equals(user.Id));
    }

    public async Task Delete(long id)
    {
        var expense = await _dbContext.Expenses.FindAsync(id);
        _dbContext.Expenses.Remove(expense!);
    }

    public void Update(Expense request) => _dbContext.Expenses.Update(request);


    private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense() => _dbContext.Expenses.Include(x => x.Tags);

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses.AsNoTracking()
            .Where(expense => expense.UserId.Equals(user.Id) && expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(order => order.Date)
            .ThenBy(then => then.Title)
            .ToListAsync();
    }
}