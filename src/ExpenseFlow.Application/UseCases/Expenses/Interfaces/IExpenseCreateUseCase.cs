using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseCreateUseCase
{
    Task<ResponseExpensesCreatedJson> Create(RequestExpensesCreatedModel request);
}