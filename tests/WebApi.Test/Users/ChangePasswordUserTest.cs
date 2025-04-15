using CommonTests.Requests;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users;
public class ChangePasswordUserTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/user/change-password";
    private readonly string _token;
    private readonly string _email;
    private readonly string _password;

    public ChangePasswordUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _password = webApplicationFactory.User_Team_Member.GetPassword();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.OldPassword = _password;

        var result = await DoPut(METHOD, request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginUserJson
        {
            Email = _email,
            Password = _password
        };

        result = await DoPost("api/login", loginRequest);
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        result = await DoPost("api/login", loginRequest);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Password_Different_Current_Password(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        var result = await DoPut(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("OLDPASSWORD_DIFFERENT_CURRENT_NEWPASSWORD", new CultureInfo(culture));

        firstErros.GetProperty("message").GetString().Should().Be(expectedMessage);
    }
}