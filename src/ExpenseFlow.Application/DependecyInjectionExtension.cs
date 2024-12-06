using ExpenseFlow.Application.AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Application;
public static class DependecyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMaper(services);
        AddUseCases(services);
    }

    public static void AddAutoMaper(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddAutoMapper(typeof(AutoMapping));
    }

    public static void AddUseCases(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddScoped<IExpenseCreateUseCase, ExpenseCreatedUseCase>();
        serviceDescriptors.AddScoped<IExpenseGetAllUseCase, ExpenseGetAllUseCase>();
        serviceDescriptors.AddScoped<IExpenseGetByIdUseCase, ExpenseGetByIdUseCase>();
        serviceDescriptors.AddScoped<IExpenseDeleteUseCase, ExpenseDeleteUseCase>();
        serviceDescriptors.AddScoped<IExpenseUpdateUseCase, ExpenseUpdateUseCase>();
    }
}