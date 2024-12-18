using ExpenseFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace ExpenseFlow.Infrastructure.Security;
internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        string passwordHash = BC.HashPassword(password);
        return passwordHash;
    }

    public bool VerificationPassword(string password, string passwordHash) => BC.Verify(password, passwordHash);
}