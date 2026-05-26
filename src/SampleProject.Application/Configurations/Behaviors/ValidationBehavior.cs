using SampleProject.Application.Configurations.Mediators;
using SampleProject.Application.SeedWork;
using SampleProject.Application.Shared.Exceptions;

namespace SampleProject.Application.Configurations.Behaviors;

public class ValidationBehavior<TRequest>(IEnumerable<IRequestValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest> where TRequest : IRequest
{
    public async Task HandleAsync(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken = default)
    {
        var validatorList = validators.ToList();
        if (validatorList.Count == 0)
        {
            await next();
            return;
        }

        var errors = validatorList
            .SelectMany(v => v.Validate(request))
            .GroupBy(kv => kv.Key)
            .ToDictionary(
                g => g.Key,
                IReadOnlyCollection<string> (g) => g.SelectMany(kv => kv.Value).ToList());

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        await next();
    }
}

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IRequestValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        var validatorList = validators.ToList();
        if (validatorList.Count == 0)
        {
            return await next();
        }

        var errors = validatorList
            .SelectMany(v => v.Validate(request))
            .GroupBy(kv => kv.Key)
            .ToDictionary(
                g => g.Key,
                IReadOnlyCollection<string> (g) => g.SelectMany(kv => kv.Value).ToList());

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}