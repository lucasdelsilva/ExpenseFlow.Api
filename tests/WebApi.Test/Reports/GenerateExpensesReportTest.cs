using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Reports;
public class GenerateExpensesReportTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/report";

    private readonly string _adminToken;
    private readonly string _memberToken;
    private readonly DateTime _date;

    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _memberToken = webApplicationFactory.User_Team_Member.GetToken();
        _date = webApplicationFactory.Expense_TeamAdmin.GetDate();
    }

    [Fact]
    public async Task Success_PDF()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?date={_date:yyyy/MM}", token: _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?date=2025/05", token: _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?date={_date:yyyy/MM}", token: _adminToken);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_PDF()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?date={_date:yyyy/MM}", token: _memberToken);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?date={_date:yyyy/MM}", token: _memberToken);
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}