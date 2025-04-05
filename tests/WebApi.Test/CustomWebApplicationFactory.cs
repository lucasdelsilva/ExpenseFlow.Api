using CommonTests.Entities;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Security.Cryptography;
using ExpenseFlow.Domain.Security.Tokens;
using ExpenseFlow.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Expense? _expense;
    private User? _user;

    private string? _password;
    private string? _token;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<ApplicationDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();

                StartDataBase(dbContext, passwordEncripter);

                var tokenGenerater = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
                _token = tokenGenerater.GeneratorToken(_user!);
            });
    }

    public string GetEmail() => _user!.Email;
    public string GetName() => _user!.Name;
    public string GetPassword() => _password!;
    public string GetToken() => _token!;
    public long GetExpenseId() => _expense!.Id;

    private void AddExpenses(ApplicationDbContext dbContext, User user)
    {
        _expense = ExpenseBuilder.Build(user);
        dbContext.Expenses.Add(_expense);
    }

    private void StartDataBase(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        AddUsers(dbContext, passwordEncripter);
        AddExpenses(dbContext, _user!);

        dbContext.SaveChanges();
    }

    private void AddUsers(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;

        _user.Password = passwordEncripter.Encrypt(_user.Password);
        dbContext.Users.Add(_user);
    }
}