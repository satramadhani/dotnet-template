using SampleProject.Infrastructure.Mediators;

namespace SampleProject.API.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.ConfigureMediator();

        return services;
    }
}
