namespace SampleProject.Application.Configurations.Mediators;

public interface IPipelineBehavior<in TRequest>
{
    Task HandleAsync(TRequest request, RequestHandlerDelegate next, CancellationToken cancellationToken = default);
}

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken = default);
}

public delegate Task RequestHandlerDelegate();

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
