using Microsoft.Extensions.DependencyInjection;
using SampleProject.Application.Configurations.Behaviors;
using SampleProject.Application.Configurations.Mediators;
using SampleProject.Application.SeedWork;
using System.Reflection;

namespace SampleProject.Infrastructure.Mediators;

public static class MediatorExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ConfigureMediator()
        {
            var assembly = typeof(IMediator).Assembly;

            services.AddMediator(assembly)
                .AddMediatorBehavior(typeof(ValidationBehavior<>))
                .AddMediatorBehavior(typeof(ValidationBehavior<,>));
            
            return services;
        }

        private IServiceCollection AddMediator(params Assembly[] assemblies)
        {
            services.AddScoped<IMediator, MediatorCompiledVersion>();

            var targets = new[]
            {
                typeof(IRequestHandler<>),
                typeof(IRequestHandler<,>),

                typeof(IRequestValidator<>)
            };

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface)
                    .ToList();

                var handlers = types
                    .SelectMany(t => t.GetInterfaces(), (type, iface) => new { type, iface })
                    .Where(x =>
                        x.iface.IsGenericType &&
                        targets.Contains(x.iface.GetGenericTypeDefinition()))
                    .ToList();

                foreach (var handler in handlers)
                {
                    services.AddTransient(handler.iface, handler.type);
                }
            }

            return services;
        }

        private IServiceCollection AddMediatorBehavior(Type behaviorType)
        {
            var targets = new[]
            {
                typeof(IPipelineBehavior<>),
                typeof(IPipelineBehavior<,>)
            };

            var iface = behaviorType
                .GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType &&
                    targets.Contains(i.GetGenericTypeDefinition()));

            if (iface == null)
            {
                throw new InvalidOperationException($"The behavior type {behaviorType.Name} does not implement " +
                                                    $"a valid IPipelineBehavior interface.");
            }
            
            services.AddTransient(iface.GetGenericTypeDefinition(), behaviorType);
            return services;
        }
    }

}
