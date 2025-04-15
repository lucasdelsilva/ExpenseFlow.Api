using AutoMapper;
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
        CreateMap<RequestExpenseCreateOrUpdateJson, Expense>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Email, src => src.MapFrom(x => x.Email.ToLower()))
            .ForMember(dest => dest.UserId, src => src.MapFrom(x => Guid.NewGuid()))
            .ForMember(dest => dest.Password, conf => conf.Ignore());
    }

    private void EntityToResponse()
    {
        // Mapeamento de Expense para ResponseExpensesCreatedModel
        CreateMap<Expense, ResponseExpenseCreateJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpensesJson>();
        CreateMap<Expense, ResponseExpenseJson>();
        CreateMap<User, ResponseProfileUserJson>();
    }
}