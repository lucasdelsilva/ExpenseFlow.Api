using CommonTests.AutoMapper;
using CommonTests.Entities;
using CommonTests.Repositories;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Domain.Entities;
using FluentAssertions;

namespace UseCases.Tests.Expenses;
public class ExpenseGetAllUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.GetAll();
        result.Should().NotBeNull();

        result.Expenses.Should().NotBeNullOrEmpty().And.AllSatisfy(expense =>
        {
            expense.Id.Should().BeGreaterThan(0);
            expense.Title.Should().NotBeNullOrEmpty();
            expense.Amount.Should().BeGreaterThan(0);
        });
    }

    private ExpenseGetAllUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
        var mapper = AutoMapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new ExpenseGetAllUseCase(repository, mapper, loggedUser);
    }
}
