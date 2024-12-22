using Bogus;
using ExpenseFlow.Communication.Request;

namespace CommonTests.Requests;
public class RequestLoginUserJsonBuilder
{
    public static RequestLoginUserJson Build()
    {
        return new Faker<RequestLoginUserJson>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "#Aa1"));
    }
}