using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.User.Validator;
using ExpenseFlow.Exception;
using FluentAssertions;

namespace Validators.Tests.UseCases.User;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateValidator();
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = validator.Validate(request);

        response.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        var validator = new UpdateValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
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
        var validator = new UpdateValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email!;

        var response = validator.Validate(request);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "teste.com";

        var response = validator.Validate(request);
        response.IsValid.Should().BeFalse();
        response.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}
