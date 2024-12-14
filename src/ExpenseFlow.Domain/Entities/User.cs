using ExpenseFlow.Domain.Enums;

namespace ExpenseFlow.Domain.Entities;
public class User
{
    public Guid UserId { get; set; }
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.TEAM_MEMBER;
}