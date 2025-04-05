using CommonTests.AutoMapper;
using CommonTests.Entities;
using CommonTests.Repositories;
using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;
using System.Text.Json;

namespace UseCases.Tests.Expenses;
public class ExpenseCreatedUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Create(request);
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);

    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();

        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Create(request);
        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();

        var result = exception.Which.GetErros().FirstOrDefault();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(result));

        dict!["PropertyName"].Should().Be("Title");
        dict["Message"].Should().Be(ResourceErrorMessages.TITLE_REQUIRED);
    }

    private ExpenseCreatedUseCase CreateUseCase(User user)
    {
        var repository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = AutoMapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new ExpenseCreatedUseCase(repository, unitOfWork, mapper, loggedUser);
    }
}