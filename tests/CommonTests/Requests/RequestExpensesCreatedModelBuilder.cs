using Bogus;
using ExpenseFlow.Communication.Enums.Expenses;
using ExpenseFlow.Communication.Request;

namespace CommonTests.Requests;
public class RequestExpensesCreatedModelBuilder
{
    public static RequestExpenseCreateOrUpdateJson Request()
    {
        return new Faker<RequestExpenseCreateOrUpdateJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 100))
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>());
    }
}