using ExpenseFlow.Communication.Request;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;
public class ExpenseFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    public ExpenseFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        CultureRequest(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string requestUri, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        CultureRequest(culture);

        return await _httpClient.DeleteAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> DoGet(string requestUri, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        CultureRequest(culture);

        return await _httpClient.GetAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> DoPut(string requestUri, RequestExpenseCreateOrUpdateJson request, string token, string culture = "en")
    {
        AuthorizeRequest(token);
        CultureRequest(culture);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoPut(string requestUri, RequestChangePasswordJson request, string token, string culture = "en")
    {
        AuthorizeRequest(token);
        CultureRequest(culture);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }
    protected async Task<HttpResponseMessage> DoPut(string requestUri, RequestUpdateProfileUserJson request, string token, string culture = "en")
    {
        AuthorizeRequest(token);
        CultureRequest(culture);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    private void AuthorizeRequest(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void CultureRequest(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}