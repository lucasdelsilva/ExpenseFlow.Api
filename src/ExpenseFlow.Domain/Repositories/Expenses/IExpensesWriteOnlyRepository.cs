using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Repositories.Expenses;
public interface IExpensesWriteOnlyRepository
{
    Task Create(Expense expense);
    Task Delete(long id);
    void Update(Expense request);
}