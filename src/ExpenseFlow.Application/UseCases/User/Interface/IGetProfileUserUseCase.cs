using ExpenseFlow.Communication.Response;

namespace ExpenseFlow.Application.UseCases.User.Interface;
public interface IGetProfileUserUseCase
{
    Task<ResponseProfileUserJson> GetProfile();
}