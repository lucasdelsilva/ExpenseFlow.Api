﻿using AutoMapper;
using ExpenseFlow.Application.UseCases.Expenses.Interfaces;
using ExpenseFlow.Application.UseCases.Expenses.Validator;
using ExpenseFlow.Communication.Request;
using ExpenseFlow.Communication.Response;
using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Domain.Repositories.Expenses;
using ExpenseFlow.Domain.Repositories.Interfaces;
using ExpenseFlow.Exception.ExceptionBase;

namespace ExpenseFlow.Application.UseCases.Expenses;
public class ExpenseCreatedUseCase : IExpenseCreateUseCase
{
    private readonly IExpensesWriteOnlyRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ExpenseCreatedUseCase(IExpensesWriteOnlyRepository expensesRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _expensesRepository = expensesRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseCreateJson> Create(RequestExpenseCreateOrUpdateJson request)
    {
        Validate(request);

        var expense = _mapper.Map<Expense>(request);

        await _expensesRepository.Create(expense);
        await _unitOfWork.Commit();

        return await Task.FromResult(_mapper.Map<ResponseExpenseCreateJson>(expense));
    }

    private void Validate(RequestExpenseCreateOrUpdateJson request)
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var objErros = new List<Object>();
            foreach (var item in result.Errors)
            {
                var objModel = new { item.PropertyName, Message = item.ErrorMessage };
                objErros.Add(objModel);
            }
            throw new ErrorOnValidationException(objErros);
        }
    }
}
