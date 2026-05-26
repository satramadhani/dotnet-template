using SampleProject.Application.Configurations.Mediators;

namespace SampleProject.Application.SeedWork;

public interface IQuery<out TResult> : IRequest<TResult>
{

}
