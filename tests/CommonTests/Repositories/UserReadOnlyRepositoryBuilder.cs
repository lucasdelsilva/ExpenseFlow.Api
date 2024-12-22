﻿using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.User;
using Moq;

namespace CommonTests.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(repository => repository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}