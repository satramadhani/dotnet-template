using SampleProject.Application.Configurations.Generators;
using SampleProject.Application.SeedWork;

namespace SampleProject.Application.Providers.Tests.Queries;

public record TestIdGeneratorQuery() : IQuery<long>;

public class TestIdGeneratorQueryHandler(IIdGenerator idGenerator) : IQueryHandler<TestIdGeneratorQuery, long>
{
    public async Task<long> HandleAsync(TestIdGeneratorQuery request, CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return idGenerator.Generate();
    }
}
