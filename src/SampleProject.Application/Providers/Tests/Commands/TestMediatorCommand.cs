using SampleProject.Application.Providers.Tests.Dtos;
using SampleProject.Application.SeedWork;

namespace SampleProject.Application.Providers.Tests.Commands;

public record TestMediatorCommand() : ICommand<TestMediatorDto>;

public class TestMediatorCommandHandler() : ICommandHandler<TestMediatorCommand, TestMediatorDto>
{
    public async Task<TestMediatorDto> HandleAsync(TestMediatorCommand request, CancellationToken cancellationToken = default)
    {
        // Simulate asynchronous process.
        await Task.CompletedTask;

        return new TestMediatorDto();
    }
}
