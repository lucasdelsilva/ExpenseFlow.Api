using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.User.Interface;
public interface ILoginUserUseCase
{
    Task<ResponseRegisterUserJson> Login(RequestLoginUserJson request);
}
