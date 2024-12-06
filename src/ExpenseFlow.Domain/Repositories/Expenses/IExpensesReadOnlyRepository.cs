using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Repositories.Expenses;
public interface IExpensesReadOnlyRepository
{
    Task<List<Expense>> GetAll();
    Task<Expense> GetById(long id);
    Task<Expense> UpdateOrRemoveGetById(long id);
}