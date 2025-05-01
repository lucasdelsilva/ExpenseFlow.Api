using CommonTests.Entities;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Enums;
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
    public ExpenseIdentityManager Expense_TeamAdmin { get; private set; } = default!;
    public ExpenseIdentityManager Expense_TeamMember { get; private set; } = default!;
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

    private void StartDataBase(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, accessTokenGenerator);
        var expensesMember = AddExpenses(dbContext, userTeamMember, expenseId: 1, tagId: 1);
        Expense_TeamMember = new ExpenseIdentityManager(expensesMember);

        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, accessTokenGenerator);
        var expensesAdmin = AddExpenses(dbContext, userAdmin, expenseId: 2, tagId: 2);
        Expense_TeamAdmin = new ExpenseIdentityManager(expensesAdmin);

        dbContext.SaveChanges();
    }

    private Expense AddExpenses(ApplicationDbContext dbContext, User user, long expenseId, long tagId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        foreach (var tag in expense.Tags)
        {
            tag.Id = tagId;
            tag.ExpenseId = expense.Id;
        }

        dbContext.Expenses.Add(expense);

        return expense;
    }

    private User AddUserTeamMember(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;
        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.GeneratorToken(user);
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(ApplicationDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);
        dbContext.Users.Add(user);

        var token = accessTokenGenerator.GeneratorToken(user);
        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }
}