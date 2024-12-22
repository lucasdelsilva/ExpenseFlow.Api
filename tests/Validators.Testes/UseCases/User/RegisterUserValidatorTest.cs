using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.User.Validator;
using ExpenseFlow.Exception;
using FluentAssertions;

namespace Validators.Tests.UseCases.User;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = validator.Validate(request);

        response.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name!;

        var response = validator.Validate(request);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));

    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Email_Empty(string? email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email!;

        var response = validator.Validate(request);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "teste.com";

        var response = validator.Validate(request);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Fact]
    public void Error_Password_Empty_Not_Equals()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var response = validator.Validate(request);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().Contain(e => e.ErrorMessage.Contains(ResourceErrorMessages.INVALID_PASSWORD));
        response.Errors.Should().Contain(e => e.ErrorMessage.Contains(ResourceErrorMessages.PASSWORD_EQUAL));

    }
}
