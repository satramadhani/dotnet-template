using SampleProject.Application.Configurations.Mediators;

namespace SampleProject.Application.SeedWork;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{

}
