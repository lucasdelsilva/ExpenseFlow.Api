using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.User.Validator;
using ExpenseFlow.Exception;
using FluentAssertions;

namespace Validators.Tests.UseCases.User;
public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = validator.Validate(request);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("      ")]
    [InlineData(null)]
    public void Error_NewPassword_Empty(string? newPassword)
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = newPassword!;

        var result = validator.Validate(request);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}
