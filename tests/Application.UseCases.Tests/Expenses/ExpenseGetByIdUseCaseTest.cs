using CommonTests.AutoMapper;
using CommonTests.Entities;
using CommonTests.Repositories;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;

namespace UseCases.Tests.Expenses;
public class ExpenseGetByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);
        var result = await useCase.GetById(expense.Id);

        result.Should().NotBeNull();

        result.Id.Should().Be(expense.Id);
        result.Title.Should().Be(expense.Title);
        result.Description.Should().Be(expense.Description);
        result.Date.Should().Be(expense.Date);
        result.Amount.Should().Be(expense.Amount);
        result.PaymentType.Should().Be((ExpenseFlow.Communication.Enums.Expenses.PaymentType)expense.PaymentType);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.GetById(id: 100);
        var result = await act.Should().ThrowAsync<NotFoundException>();

        result.Where(ex => ex.GetErros().Count == 1 && ex.GetErros().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private ExpenseGetByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = AutoMapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new ExpenseGetByIdUseCase(repository, mapper, loggedUser);
    }
}