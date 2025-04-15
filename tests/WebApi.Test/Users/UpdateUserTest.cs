using CommonTests.Requests;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users;
public class UpdateUserTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/User";
    private readonly string _token;

    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var result = await DoPut(METHOD, request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        firstErros.GetProperty("message").GetString().Should().Be(expectedMessage);
        firstErros.GetProperty("propertyName").GetString().Should().Be("Name");
    }
}