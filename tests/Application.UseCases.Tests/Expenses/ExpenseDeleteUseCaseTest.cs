using CommonTests.Entities;
using CommonTests.Repositories;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;

namespace UseCases.Tests.Expenses;
public class ExpenseDeleteUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);
        var result = async () => await useCase.Delete(expense.Id);

        await result.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);
        var act = async () => await useCase.Delete(id: 100);

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErros().Count == 1 && ex.GetErros().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private ExpenseDeleteUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryRead = new ExpensesReadOnlyRepositoryBuilder().UpdateOrRemoveGetById(user, expense).Build();
        var repositoryWrite = ExpensesWriteOnlyRepositoryBuilder.Build();

        var unitOfWord = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new ExpenseDeleteUseCase(repositoryWrite, repositoryRead, unitOfWord, loggedUser);
    }
}