using SampleProject.Application.Configurations.Mediators;

namespace SampleProject.Application.SeedWork;

public interface ICommand : IRequest
{

}

public interface ICommand<out TResult> : IRequest<TResult>
{

}
