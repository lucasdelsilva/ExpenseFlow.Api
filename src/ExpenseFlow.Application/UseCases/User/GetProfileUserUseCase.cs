using AutoMapper;
using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Services.LoggedUser;

namespace ExpenseFlow.Application.UseCases.User;
public class GetProfileUserUseCase : IGetProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetProfileUserUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseProfileUserJson> GetProfile()
    {
        var user = await _loggedUser.Get();
        return _mapper.Map<ResponseProfileUserJson>(user);
    }
}