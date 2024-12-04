using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Application;
public static class DependecyInjectionExtension
{
    public static void AddApplication(this IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddScoped<IExpenseCreatedUserCase, ExpenseCreatedUseCase>();
    }
}
