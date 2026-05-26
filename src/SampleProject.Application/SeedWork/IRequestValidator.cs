namespace SampleProject.Application.SeedWork;

public interface IRequestValidator<in TRequest>
{
    Dictionary<string, IReadOnlyCollection<string>> Validate(TRequest request);
}
