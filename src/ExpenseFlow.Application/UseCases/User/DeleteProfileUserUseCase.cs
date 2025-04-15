using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Domain.Repositories.User;
using ExpenseFlow.Domain.Services.LoggedUser;

namespace ExpenseFlow.Application.UseCases.User;
public class DeleteProfileUserUseCase : IDeleteProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProfileUserUseCase(ILoggedUser loggedUser, IUserReadOnlyRepository userRepository, IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task DeleteProfile()
    {
        var user = await _loggedUser.Get();

        await _userRepository.Delete(user);
        await _unitOfWork.Commit();
    }
}