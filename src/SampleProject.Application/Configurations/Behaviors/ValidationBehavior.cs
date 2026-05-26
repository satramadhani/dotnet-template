using FluentValidation;
using SampleProject.Application.Configurations.Mediators;
using SampleProject.Application.SeedWork;
using SampleProject.Application.Shared.Helpers;

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

        var failures = validatorList
            .SelectMany(v => ValidationHelper.ToValidationFailures(v.Validate(request)))
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
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

        var failures = validatorList
            .SelectMany(v => ValidationHelper.ToValidationFailures(v.Validate(request)))
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}