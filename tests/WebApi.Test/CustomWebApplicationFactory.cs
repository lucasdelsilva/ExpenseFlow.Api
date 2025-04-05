using CommonTests.Entities;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Security.Cryptography;
using ExpenseFlow.Domain.Security.Tokens;
using ExpenseFlow.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense { get; private set; } = default!;
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

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
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDataBase(dbContext, passwordEncripter, accessTokenGenerator);
            });
    }
    private void AddExpenses(ApplicationDbContext dbContext, User user)
    {
        var expense = ExpenseBuilder.Build(user);
        dbContext.Expenses.Add(expense);

        Expense = new ExpenseIdentityManager(expense);
    }

    private void StartDataBase(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
        AddExpenses(dbContext, user);

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.GeneratorToken(user);
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }
}