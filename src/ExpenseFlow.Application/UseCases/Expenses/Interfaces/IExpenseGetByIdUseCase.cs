using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.Expenses.Interfaces;
public interface IExpenseGetByIdUseCase
{
    Task<ResponseExpenseJson> GetById(long id);
}
