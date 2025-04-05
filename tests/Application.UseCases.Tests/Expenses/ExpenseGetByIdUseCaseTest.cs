using ExpenseFlow.Application.UseCases.Expenses;

namespace UseCases.Tests.Expenses;
public class ExpenseGetByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {

    }
    [Fact]
    public async Task Error_Expense_Not_Found()
    {

    }

    private ExpenseGetByIdUseCase CreateUseCase()
    {
        //var repository = new ExpensesReadOnlyRepositoryBuilder
        //var mapper = new Mock<IMapper>();
        //var loggedUser = new Mock<ILoggedUser>();
        //return new ExpenseGetByIdUseCase(repository, mapper.Object, loggedUser.Object);

        return new ExpenseGetByIdUseCase(null, null, null);
    }
}
