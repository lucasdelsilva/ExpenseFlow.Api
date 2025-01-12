using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Security.Tokens;
using ExpenseFlow.Domain.Services.LoggedUser;
using ExpenseFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExpenseFlow.Infrastructure.Services.LoggedUser;
internal class LoggedUser : ILoggedUser
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(ApplicationDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;

    }
    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(ClaimTypes.Sid)).Value;

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.UserId.Equals(Guid.Parse(identifier)));
    }
}