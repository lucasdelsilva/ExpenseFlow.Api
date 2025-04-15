using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Application.UseCases.User.Validator;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Domain.Repositories.User;
using ExpenseFlow.Domain.Services.LoggedUser;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.User;
public class UpdateProfileUserUseCase : IUpdateProfileUserUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public UpdateProfileUserUseCase(IUnitOfWork unitOfWork, IUserReadOnlyRepository userReadOnlyRepository, ILoggedUser loggedUser)
    {
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task UpdateProfile(RequestUpdateProfileUserJson request)
    {
        var loggedUser = await _loggedUser.Get();
        await Validate(request, loggedUser.Email);

        var user = await _userReadOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _userReadOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateProfileUserJson request, string email)
    {
        var validator = new UpdateValidator();
        var result = validator.Validate(request);

        if (email.Equals(request.Email) == false)
        {
            var existEmail = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (existEmail)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure("Email", ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var objErros = new List<Object>();
            foreach (var item in result.Errors)
            {
                var objModel = new { item.PropertyName, Message = item.ErrorMessage };
                objErros.Add(objModel);
            }
            throw new ErrorOnValidationException(objErros);
        }
    }
}
