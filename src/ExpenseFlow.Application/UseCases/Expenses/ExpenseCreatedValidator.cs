using ExpenseFlow.Communication.Request;
using FluentValidation;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseCreatedValidator : AbstractValidator<RequestExpensesCreatedModel>
{
    public ExpenseCreatedValidator()
    {
        RuleFor(prop => prop.Title).NotEmpty().WithMessage("The title is required.");
        RuleFor(prop => prop.Amount).GreaterThan(0).WithMessage("The Amount must be greater than zero.");
        RuleFor(prop => prop.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot be for the future.");
        RuleFor(prop => prop.PaymentType).IsInEnum().WithMessage("Payment Type is not valid.");
    }
}