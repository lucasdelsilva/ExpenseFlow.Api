using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseCreatedUserCase
{
    Task<ResponseExpensesCreatedModel> Execute(RequestExpensesCreatedModel request);
}