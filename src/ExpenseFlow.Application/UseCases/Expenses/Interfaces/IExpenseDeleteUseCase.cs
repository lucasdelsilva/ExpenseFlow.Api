namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseDeleteUseCase
{
    Task Delete(long id);
}