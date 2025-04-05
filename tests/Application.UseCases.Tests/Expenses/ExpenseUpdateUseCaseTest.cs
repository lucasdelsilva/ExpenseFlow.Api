using CommonTests.AutoMapper;
using CommonTests.Entities;
using CommonTests.Repositories;
using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Enums;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;
using System.Text.Json;

namespace UseCases.Tests.Expenses;
public class ExpenseUpdateUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        request.Title = "New String!";

        var useCase = CreateUseCase(loggedUser, expense);
        var result = async () => await useCase.Update(expense.Id, request);

        await result.Should().NotThrowAsync();

        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Date.Should().Be(request.Date);
        expense.Amount.Should().Be(request.Amount);
        expense.PaymentType.Should().Be((PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser, expense);
        var act = async () => await useCase.Update(expense.Id, request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();

        var result = exception.Which.GetErros().FirstOrDefault();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(result));

        dict!["PropertyName"].Should().Be("Title");
        dict["Message"].Should().Be(ResourceErrorMessages.TITLE_REQUIRED);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();

        var useCase = CreateUseCase(loggedUser);
        var act = async () => await useCase.Update(id: 100, request);

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErros().Count == 1 && ex.GetErros().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private ExpenseUpdateUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryRead = new ExpensesReadOnlyRepositoryBuilder().UpdateOrRemoveGetById(user, expense).Build();
        var repositoryWrite = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = AutoMapperBuilder.Build();
        var unitOfWord = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new ExpenseUpdateUseCase(repositoryWrite, repositoryRead, unitOfWord, mapper, loggedUser);
    }
}
