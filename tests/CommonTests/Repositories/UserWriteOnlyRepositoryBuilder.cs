﻿using ExpenseFlow.Domain.Repositories.User;
using Moq;

namespace CommonTests.Repositories;
public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}