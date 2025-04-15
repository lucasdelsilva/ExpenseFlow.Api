using CommonTests.AutoMapper;
using CommonTests.Entities;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Domain.Entities;
using FluentAssertions;

namespace UseCases.Tests.Users;
public class GetProfileUserUseCaseTest
{

    [Fact]
    public async Task Success()
     {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.GetProfile();

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }

    private GetProfileUserUseCase CreateUseCase(User user)
    {
        var mapper = AutoMapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetProfileUserUseCase(loggedUser, mapper);
    }
}