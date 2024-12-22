using ExpenseFlow.Domain.Repositories.Interfaces;
using Moq;

namespace CommonTests.Repositories;
public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var moq = new Mock<IUnitOfWork>();
        return moq.Object;
    }
}