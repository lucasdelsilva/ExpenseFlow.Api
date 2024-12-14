using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.User.Interface;
public interface IRegisterUserUseCase
{
    Task<ResponseRegisterUserJson> Register(RequestRegisterUserJson request);
}
