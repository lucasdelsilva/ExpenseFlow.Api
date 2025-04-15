using CommonTests.Requests;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users;
public class LoginUserTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/login";
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public LoginUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.User_Team_Member.GetEmail();
        _name = webApplicationFactory.User_Team_Member.GetName();
        _password = webApplicationFactory.User_Team_Member.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginUserJson
        {
            Email = _email,
            Password = _password
        };

        var result = await DoPost(requestUri: METHOD, request: request);
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(_name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Erro_Login_Invalid(string culture)
    {
        var request = RequestLoginUserJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request, culture: culture);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        resultErros.Should().HaveCount(1).And.Contain(e => e.GetString()!.Equals(expectedMessage));
    }
}
