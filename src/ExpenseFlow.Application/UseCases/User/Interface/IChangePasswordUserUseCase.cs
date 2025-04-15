using ExpenseFlow.Communication.Request;

namespace ExpenseFlow.Application.UseCases.User.Interface;
public interface IChangePasswordUserUseCase
{
    Task ChangePassword(RequestChangePasswordJson request);
}