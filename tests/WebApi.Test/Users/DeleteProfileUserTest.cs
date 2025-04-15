using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users;
public class DeleteProfileUserTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/user";
    private readonly string _token;

    public DeleteProfileUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(METHOD, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}