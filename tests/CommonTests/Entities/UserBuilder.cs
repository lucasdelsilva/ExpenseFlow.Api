using Bogus;
using CommonTests.Cryptography;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Enums;

namespace CommonTests.Entities;
public class UserBuilder
{
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Build();

        var user = new Faker<User>()
           .RuleFor(u => u.Id, _ => 1)
           .RuleFor(u => u.Name, faker => faker.Person.FirstName)
           .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
           .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
           .RuleFor(u => u.UserId, _ => Guid.NewGuid())
           .RuleFor(u => u.Role, _ => role);

        return user;
    }
}