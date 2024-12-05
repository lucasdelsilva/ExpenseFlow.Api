using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseGetAllUseCase
{
    Task<ResponseExpensesJson> GetAll();
}