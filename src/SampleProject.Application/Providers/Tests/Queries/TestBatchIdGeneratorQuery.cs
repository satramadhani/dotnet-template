using SampleProject.Application.Configurations.Generators;
using SampleProject.Application.SeedWork;
using SampleProject.Application.SeedWork.Validators;

namespace SampleProject.Application.Providers.Tests.Queries;

public record TestBatchIdGeneratorQuery(int BatchSize) : IQuery<IReadOnlyList<long>>;

public class TestBatchIdGeneratorQueryHandler(IIdGenerator generator)
    : IQueryHandler<TestBatchIdGeneratorQuery, IReadOnlyList<long>>
{
    public async Task<IReadOnlyList<long>> HandleAsync(TestBatchIdGeneratorQuery request,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return generator.GenerateBatch(request.BatchSize);
    }
}

// A sample of validator using custom validation base class.
public class TestBatchIdGeneratorValidator : ValidatorBase<TestBatchIdGeneratorQuery>
{
    protected override void DoValidate(TestBatchIdGeneratorQuery request)
    {
        switch (request.BatchSize)
        {
            case <= 0:
                AddError(nameof(request.BatchSize), "Batch size must be greater than 0.");
                break;
            case > 4096:
                AddError(nameof(request.BatchSize), "Batch size must be less than or equal to 4096.");
                break;
        }
    }
}
