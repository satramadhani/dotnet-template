namespace SampleProject.Application.Configurations.Mediators;

public interface IMediator
{
    Task SendAsync(IRequest request, CancellationToken cancellationToken = default);

    Task<TResult> SendAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default);
}
