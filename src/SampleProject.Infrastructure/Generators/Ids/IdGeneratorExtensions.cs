using Microsoft.Extensions.DependencyInjection;
using SampleProject.Application.Configurations.Generators;

namespace SampleProject.Infrastructure.Generators.Ids;

public static class IdGeneratorExtensions
{
    public static IServiceCollection ConfigureIdGenerator(this IServiceCollection services)
    {
        services.AddSingleton<IIdGenerator, IdGenerator>();
        return services;
    }
}
