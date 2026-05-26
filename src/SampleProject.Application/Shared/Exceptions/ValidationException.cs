namespace SampleProject.Application.Shared.Exceptions;

public class ValidationException(Dictionary<string, IReadOnlyCollection<string>> errors) 
    : Exception("One or more validation errors occurred.")
{
    public Dictionary<string, IReadOnlyCollection<string>> Errors { get; } = errors;

}
