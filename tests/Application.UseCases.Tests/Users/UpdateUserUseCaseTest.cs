using CommonTests.Entities;
using CommonTests.Repositories;
using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;
using System.Text.Json;

namespace UseCases.Tests.Users;
public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreatedUseCase(user);

        var act = async () => await useCase.UpdateProfile(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreatedUseCase(user);

        var act = async () => await useCase.UpdateProfile(request);
        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();

        var result = exception.Which.GetErros().FirstOrDefault();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(result));

        dict!["PropertyName"].Should().Be("Name");
        dict["Message"].Should().Be(ResourceErrorMessages.NAME_EMPTY);
    }

    private UpdateProfileUserUseCase CreatedUseCase(User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateReadOnlyRepository = new UserReadOnlyRepositoryBuilder().UpdateUser(user).Build();


        if (string.IsNullOrWhiteSpace(email) == false)
            updateReadOnlyRepository.ExistActiveUserWithEmail(email);

        return new UpdateProfileUserUseCase(unitOfWork, updateReadOnlyRepository, loggedUser);
    }
}