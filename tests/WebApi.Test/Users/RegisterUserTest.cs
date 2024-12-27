using CommonTests.Requests;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace WebApi.Test.Users;
public class RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/User";
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        firstErros.GetProperty("message").GetString().Should().Be(ResourceErrorMessages.NAME_EMPTY);
        firstErros.GetProperty("propertyName").GetString().Should().Be("Name");
    }

    [Fact]
    public async Task Error_Empty_Email()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;
        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
        var firstErros = resultErros.FirstOrDefault();

        firstErros.GetProperty("message").GetString().Should().Be(ResourceErrorMessages.EMAIL_EMPTY);
        firstErros.GetProperty("propertyName").GetString().Should().Be("Email");
    }
}