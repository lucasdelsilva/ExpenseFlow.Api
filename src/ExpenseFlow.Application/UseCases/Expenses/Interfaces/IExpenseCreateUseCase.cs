using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseCreateUseCase
{
    Task<ResponseExpenseCreateJson> Create(RequestExpenseCreateOrUpdateJson request);
}