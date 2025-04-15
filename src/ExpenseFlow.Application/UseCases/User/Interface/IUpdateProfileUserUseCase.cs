using ExpenseFlow.Communication.Request;

namespace ExpenseFlow.Application.UseCases.User.Interface;
public interface IUpdateProfileUserUseCase
{
    Task UpdateProfile(RequestUpdateProfileUserJson request);
}
