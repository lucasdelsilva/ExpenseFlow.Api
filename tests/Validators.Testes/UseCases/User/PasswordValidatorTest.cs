using ExpenseFlow.Communication.Request;
using FluentAssertions;
using FluentValidation;

namespace Validators.Tests.UseCases.User;
public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid(string? password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();

        var response = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password!);
        response.Should().BeFalse();
    }
}