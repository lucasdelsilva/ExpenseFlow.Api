using CommonTests.Cryptography;
using CommonTests.Entities;
using CommonTests.Repositories;
using CommonTests.Requests;
using CommonTests.Token;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;

namespace Application.UseCases.Tests.Users;
public class LoginUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginUserJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreatedUseCase(user, request.Password);

        var response = await useCase.Login(request);

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginUserJsonBuilder.Build();

        var useCase = CreatedUseCase(user, request.Password);

        var function = async () => await useCase.Login(request);
        var exception = await function.Should().ThrowAsync<InvalidLoginException>();

        var result = exception.Which.GetErros();
        result.Should().ContainSingle().And.Contain(e => e.Equals(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginUserJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreatedUseCase(user);

        var function = async () => await useCase.Login(request);
        var exception = await function.Should().ThrowAsync<InvalidLoginException>();

        var result = exception.Which.GetErros();
        result.Should().ContainSingle().And.Contain(e => e.Equals(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    private LoginUserUseCase CreatedUseCase(User user, string? password = null)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
        var token = JwtTokenGeneratorBuild.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();

        return new LoginUserUseCase(userReadOnlyRepository, passwordEncripter, token);
    }
}
