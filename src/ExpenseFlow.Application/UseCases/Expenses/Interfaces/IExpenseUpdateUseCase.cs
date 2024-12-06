using ExpenseFlow.Communication.Request;

namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseUpdateUseCase
{
    Task Update(long id, RequestExpenseCreateOrUpdateJson request);
}