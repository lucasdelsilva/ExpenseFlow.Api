using ExpenseFlow.Application.UseCases.User.Interface;
using ExpenseFlow.Application.UseCases.User.Validator;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Domain.Repositories.User;
using ExpenseFlow.Domain.Security.Cryptography;
using ExpenseFlow.Domain.Services.LoggedUser;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.User;
public class ChangePasswordUserUseCase : IChangePasswordUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUserUseCase(ILoggedUser loggedUser, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task ChangePassword(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();
        Validate(request, loggedUser);

        var user = await _userReadOnlyRepository.GetById(loggedUser.Id);
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _userReadOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordMath = _passwordEncripter.VerificationPassword(request.OldPassword, loggedUser.Password);
        if (!passwordMath)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.OLDPASSWORD_DIFFERENT_CURRENT_NEWPASSWORD));

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