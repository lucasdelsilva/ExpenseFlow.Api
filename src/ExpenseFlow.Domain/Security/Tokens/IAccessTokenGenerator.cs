using ExpenseFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseFlow.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    string GeneratorToken(User user);
}