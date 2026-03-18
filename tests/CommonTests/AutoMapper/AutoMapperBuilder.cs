using AutoMapper;
using ExpenseFlow.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CommonTests.AutoMapper;

public class AutoMapperBuilder
{
    public static IMapper Build()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(cfg => { }, typeof(AutoMapping));

        var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IMapper>();
    }
}