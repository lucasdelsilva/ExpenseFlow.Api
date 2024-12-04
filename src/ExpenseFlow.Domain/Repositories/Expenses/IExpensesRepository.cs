using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Domain.Repositories.Expenses;
public interface IExpensesRepository
{
    Task Create(Expense expense);
}