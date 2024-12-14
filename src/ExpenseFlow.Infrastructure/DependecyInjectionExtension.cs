using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Domain.Repositories.User;
using ExpenseFlow.Domain.Security.Cryptography;
using ExpenseFlow.Infrastructure.DataAccess;
using ExpenseFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Infrastructure;
public static class DependecyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddDbContext(services, configuration);
        services.AddScoped<IPasswordEncripter, Security.BCrypt>();
    }

    private static void AddRepositories(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceDescriptors.AddScoped<IExpensesWriteOnlyRepository, ExpensesRepository>();
        serviceDescriptors.AddScoped<IExpensesReadOnlyRepository, ExpensesRepository>();
        //User
        serviceDescriptors.AddScoped<IUserReadOnlyRepository, UserRepository>();
        serviceDescriptors.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddDbContext(IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        serviceDescriptors.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, serverVersion));
    }
}