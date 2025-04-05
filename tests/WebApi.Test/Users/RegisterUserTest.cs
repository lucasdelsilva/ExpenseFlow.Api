using CommonTests.Requests;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users;
public class RegisterUserTest : ExpenseFlowClassFixture
{
    private const string METHOD = "api/User";

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory) { }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var result = await DoPost(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        firstErros.GetProperty("message").GetString().Should().Be(expectedMessage);
        firstErros.GetProperty("propertyName").GetString().Should().Be("Name");
    }

    [Theory]
    [ClassData(typeof(CultureInlinaData))]
    public async Task Error_Empty_Email(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, culture: culture);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_EMPTY", new CultureInfo(culture));

        firstErros.GetProperty("message").GetString().Should().Be(expectedMessage);
        firstErros.GetProperty("propertyName").GetString().Should().Be("Email");
    }
}