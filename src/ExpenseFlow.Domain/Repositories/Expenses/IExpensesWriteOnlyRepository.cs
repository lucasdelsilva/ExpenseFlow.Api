using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Repositories.Expenses;
public interface IExpensesWriteOnlyRepository
{
    Task Create(Expense expense);
}