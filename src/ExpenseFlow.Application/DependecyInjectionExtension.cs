﻿using ExpenseFlow.Application.AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Application.UseCases.Reports;
using ExpenseFlow.Application.UseCases.Reports.Interfaces;
using ExpenseFlow.Application.UseCases.User;
using ExpenseFlow.Application.UseCases.User.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Application;
public static class DependecyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMaper(services);
        AddUseCases(services);
    }

    public static void AddAutoMaper(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddAutoMapper(typeof(AutoMapping));
    }

    public static void AddUseCases(IServiceCollection serviceDescriptors)
    {
        //User
        serviceDescriptors.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        serviceDescriptors.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
        serviceDescriptors.AddScoped<IGetProfileUserUseCase, GetProfileUserUseCase>();
        serviceDescriptors.AddScoped<IUpdateProfileUserUseCase, UpdateProfileUserUseCase>();
        serviceDescriptors.AddScoped<IChangePasswordUserUseCase, ChangePasswordUserUseCase>();
        serviceDescriptors.AddScoped<IDeleteProfileUserUseCase, DeleteProfileUserUseCase>();

        //Expenses
        serviceDescriptors.AddScoped<IExpenseCreateUseCase, ExpenseCreatedUseCase>();
        serviceDescriptors.AddScoped<IExpenseGetAllUseCase, ExpenseGetAllUseCase>();
        serviceDescriptors.AddScoped<IExpenseGetByIdUseCase, ExpenseGetByIdUseCase>();
        serviceDescriptors.AddScoped<IExpenseDeleteUseCase, ExpenseDeleteUseCase>();
        serviceDescriptors.AddScoped<IExpenseUpdateUseCase, ExpenseUpdateUseCase>();

        //Report expenses - Excel or PDF
        serviceDescriptors.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
        serviceDescriptors.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
    }
}