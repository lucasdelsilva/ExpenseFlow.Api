using CommonTests.Requests;
using ExpenseFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses
{
    public class ExpenseCreatedTest : IClassFixture<CustomWebApplicationFactory>
    {
        private const string METHOD = "api/expenses";

        private readonly HttpClient _httpClient;
        private readonly string _token;

        public ExpenseCreatedTest(CustomWebApplicationFactory webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
            _token = webApplicationFactory.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);
            result.StatusCode.Should().Be(HttpStatusCode.Created);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        }

        [Theory]
        [ClassData(typeof(CultureInlinaData))]
        public async Task Error_Title_Name(string cultureInfo)
        {
            var request = RequestExpenseCreateOrUpdateJsonBuilder.Request();
            request.Title = string.Empty;

            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var result = await _httpClient.PostAsJsonAsync(METHOD, request);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var body = await result.Content.ReadAsStreamAsync();
            var response = await JsonDocument.ParseAsync(body);

            var resultErros = response.RootElement.GetProperty("erros").EnumerateArray();
            var firstErros = resultErros.FirstOrDefault();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));

            firstErros.GetProperty("message").GetString().Should().Be(expectedMessage);
            firstErros.GetProperty("propertyName").GetString().Should().Be("Title");
        }
    }
}