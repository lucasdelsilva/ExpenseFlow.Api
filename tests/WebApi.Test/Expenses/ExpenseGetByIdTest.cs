using CommonTests.Requests;
using ExpenseFlow.Domain.Enums;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses;
public class ExpenseGetByIdTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public ExpenseGetByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
        _expenseId = webApplicationFactory.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_expenseId);

        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        var result = await DoGet(requestUri: $"{METHOD}/{100}", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));
        resultErros.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
}