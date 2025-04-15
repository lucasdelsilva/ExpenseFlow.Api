using ExpenseFlow.Communication.Request;
using FluentValidation;

namespace ExpenseFlow.Application.UseCases.User.Validator;
public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}