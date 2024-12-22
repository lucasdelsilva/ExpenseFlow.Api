using CommonTests.AutoMapper;
using CommonTests.Cryptography;
using CommonTests.Repositories;
using CommonTests.Requests;
using CommonTests.Token;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Exception;
using ExpenseFlow.Exception.ExceptionBase;
using FluentAssertions;
using System.Text.Json;

namespace Application.UseCases.Tests.Users;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreatedUseCase();

        var response = await useCase.Register(request);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreatedUseCase();

        var function = async () => await useCase.Register(request);
        var exception = await function.Should().ThrowAsync<ErrorOnValidationException>();

        var result = exception.Which.GetErros().FirstOrDefault();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(result));

        dict!["PropertyName"].Should().Be("Name");
        dict["Message"].Should().Be(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreatedUseCase(request.Email);

        var function = async () => await useCase.Register(request);
        var exception = await function.Should().ThrowAsync<ErrorOnValidationException>();

        var result = exception.Which.GetErros().FirstOrDefault();
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(result));

        dict!["PropertyName"].Should().Be("Email");
        dict["Message"].Should().Be(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }

    private RegisterUserUseCase CreatedUseCase(string? email = null)
    {
        var mapper = AutoMapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var token = JwtTokenGeneratorBuild.Build();

        if (string.IsNullOrWhiteSpace(email) == false)
            userReadOnlyRepository.ExistActiveUserWithEmail(email);

        return new RegisterUserUseCase(mapper, passwordEncripter, userReadOnlyRepository.Build(), userWriteOnlyRepository, unitOfWork, token);
    }
}