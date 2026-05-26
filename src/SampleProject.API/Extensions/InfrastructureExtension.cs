using SampleProject.Infrastructure.Generators.Ids;
using SampleProject.Infrastructure.Mediators;

namespace SampleProject.API.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.ConfigureIdGenerator();
        services.ConfigureMediator();

        return services;
    }
}
