using Bogus;
using ExpenseFlow.Communication.Request;

namespace CommonTests.Requests;
public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(x => x.OldPassword, f => f.Internet.Password())
            .RuleFor(x => x.NewPassword, f => f.Internet.Password(prefix: "!Aa1"));
    }
}