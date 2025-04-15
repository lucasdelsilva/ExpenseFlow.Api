using ExpenseFlow.Communication.Request;
using ExpenseFlow.Exception;
using FluentValidation;

namespace ExpenseFlow.Application.UseCases.User.Validator;
public class UpdateValidator : AbstractValidator<RequestUpdateProfileUserJson>
{
    public UpdateValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(e => string.IsNullOrWhiteSpace(e.Email).Equals(false), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}