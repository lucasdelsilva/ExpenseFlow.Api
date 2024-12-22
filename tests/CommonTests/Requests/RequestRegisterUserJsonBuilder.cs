using Bogus;
using ExpenseFlow.Communication.Request;

namespace CommonTests.Requests;
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "#Aa1"))
            .RuleFor(user => user.PasswordConfirm, (faker, user) => user.Password);
    }
}
