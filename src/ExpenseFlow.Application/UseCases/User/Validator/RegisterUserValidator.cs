using ExpenseFlow.Communication.Request;
using ExpenseFlow.Exception;
using FluentValidation;

namespace ExpenseFlow.Application.UseCases.User.Validator;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        RuleFor(user => user.PasswordConfirm).Equal(user => user.Password).WithMessage(ResourceErrorMessages.PASSWORD_EQUAL);
    }
}