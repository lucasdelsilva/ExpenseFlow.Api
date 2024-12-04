using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Infrastructure.DataAccess;
using ExpenseFlow.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Infrastructure;
public static class DependecyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        AddRepositories(serviceDescriptors);
        AddDbContext(serviceDescriptors, configuration);
    }

    private static void AddRepositories(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddScoped<IExpensesRepository, ExpensesRepository>();
    }

    private static void AddDbContext(IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        serviceDescriptors.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, serverVersion));
    }
}