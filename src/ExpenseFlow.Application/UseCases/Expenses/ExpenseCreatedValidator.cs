using ExpenseFlow.Communication.Request;
using ExpenseFlow.Exception;
using FluentValidation;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseCreatedValidator : AbstractValidator<RequestExpensesCreatedModel>
{
    public ExpenseCreatedValidator()
    {
        RuleFor(prop => prop.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(prop => prop.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(prop => prop.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE);
        RuleFor(prop => prop.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}