using CommonTests.Entities;
using CommonTests.Repositories;
using ExpenseFlow.Application.UseCases.Reports;
using ExpenseFlow.Domain.Entities;
using FluentAssertions;

namespace UseCases.Tests.Reports;
public class GenerateExpensesReportPdfUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.GeneratePdfFile(DateOnly.FromDateTime(DateTime.Today));
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.GeneratePdfFile(DateOnly.FromDateTime(DateTime.Today));
        result.Should().BeEmpty();
    }

    private GenerateExpensesReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateExpensesReportPdfUseCase(repository, loggedUser);
    }
}