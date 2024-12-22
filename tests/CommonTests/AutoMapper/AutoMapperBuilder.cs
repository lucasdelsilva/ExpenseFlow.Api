using AutoMapper;
using ExpenseFlow.Application.AutoMapper;

namespace CommonTests.AutoMapper;
public class AutoMapperBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}