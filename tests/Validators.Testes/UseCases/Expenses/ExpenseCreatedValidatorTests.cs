using CommonTests.Requests;
using ExpenseFlow.Application.UseCases.Expenses.Validator;
using ExpenseFlow.Communication.Enums.Expenses;
using ExpenseFlow.Exception;
using FluentAssertions;

namespace Validators.Tests.UseCases.Expenses;
public class ExpenseCreatedValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arange
        var validator = new ExpenseValidator();
        var request = RequestExpensesCreatedModelBuilder.Request();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().HaveCount(0);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Title_Empty(string? title)
    {
        //Arange
        var validator = new ExpenseValidator();
        var request = RequestExpensesCreatedModelBuilder.Request();
        request.Title = title;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.PropertyName.Equals("Title")
            && e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arange
        var validator = new ExpenseValidator();
        var request = RequestExpensesCreatedModelBuilder.Request();
        request.Date = DateTime.UtcNow.AddDays(1);

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.PropertyName.Equals("Date")
            && e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE));
    }

    [Fact]
    public void Error_Payment_Type_Enum_Invalid()
    {
        //Arange
        var validator = new ExpenseValidator();
        var request = RequestExpensesCreatedModelBuilder.Request();
        request.PaymentType = (PaymentType)900;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.PropertyName.Equals("PaymentType")
            && e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_Invalid(decimal amount)
    {
        //Arange
        var validator = new ExpenseValidator();
        var request = RequestExpensesCreatedModelBuilder.Request();
        request.Amount = amount;

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.PropertyName.Equals("Amount")
            && e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
}