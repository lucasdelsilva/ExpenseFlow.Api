using CommonTests.Entities;
using CommonTests.Repositories;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Domain.Entities;
using FluentAssertions;

namespace UseCases.Tests.Users;
public class DeleteProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.DeleteProfile();
        await act.Should().NotThrowAsync();
    }

    private DeleteProfileUserUseCase CreateUseCase(User user)
    {
        var repository = new UserReadOnlyRepositoryBuilder().Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteProfileUserUseCase(loggedUser, repository, unitOfWork);
    }
}
