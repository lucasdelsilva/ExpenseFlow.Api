﻿using AutoMapper;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Entities;

namespace ExpenseFlow.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        // Mapeamento de RequestExpensesCreatedModel para Expense
        CreateMap<RequestExpensesCreatedModel, Expense>();
    }

    private void EntityToResponse()
    {
        // Mapeamento de Expense para ResponseExpensesCreatedModel
        CreateMap<Expense, ResponseExpensesCreatedModel>();
    }
}