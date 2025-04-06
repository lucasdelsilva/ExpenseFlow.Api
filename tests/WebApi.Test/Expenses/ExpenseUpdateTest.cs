using CommonTests.Requests;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses;
public class ExpenseUpdateTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public ExpenseUpdateTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense_TeamMember.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();

        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        request.Title = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        firstErros.GetProperty("message").GetString().Should().Be(expectedMessage);
        firstErros.GetProperty("propertyName").GetString().Should().Be("Title");
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
        var result = await DoPut(requestUri: $"{METHOD}/{100}", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));
        resultErros.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
}