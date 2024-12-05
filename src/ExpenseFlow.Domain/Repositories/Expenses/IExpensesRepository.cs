using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Repositories.Expenses;
public interface IExpensesRepository
{
    Task Create(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense> GetById(long id);
}