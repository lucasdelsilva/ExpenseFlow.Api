using CommonTests.Cryptography;
using CommonTests.Entities;
using CommonTests.Repositories;
using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Exception.ExceptionBase;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Text.Json;

namespace UseCases.Tests.Users;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreatedUseCase(user, request.OldPassword);
        var act = async () => await useCase.ChangePassword(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreatedUseCase(user);

        var act = async () => await useCase.ChangePassword(request);
        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();

        var result = exception.Which.GetErros().FirstOrDefault();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(result));

        dict!["PropertyName"].Should().Be("NewPassword");
        dict["Message"].Should().Be(ResourceErrorMessages.INVALID_PASSWORD);
    }

    private ChangePasswordUserUseCase CreatedUseCase(User user, string? password = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().UpdateUser(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

        return new ChangePasswordUserUseCase(loggedUser, userReadOnlyRepository, unitOfWork, passwordEncripter);
    }
}
