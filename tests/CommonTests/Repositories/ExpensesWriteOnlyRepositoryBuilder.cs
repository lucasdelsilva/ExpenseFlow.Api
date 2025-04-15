using ExpenseFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTests.Repositories
{
    public class ExpensesWriteOnlyRepositoryBuilder
    {
        public static IExpensesWriteOnlyRepository Build()
        {
            var mock = new Mock<IExpensesWriteOnlyRepository>();
            return mock.Object;
        }
    }
}