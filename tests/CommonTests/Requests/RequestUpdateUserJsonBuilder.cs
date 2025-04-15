using Bogus;
using ExpenseFlow.Communication.Request;

namespace CommonTests.Requests;
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateProfileUserJson Build()
    {
        return new Faker<RequestUpdateProfileUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}