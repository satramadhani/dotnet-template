using FluentValidation.Results;

namespace SampleProject.Application.Shared.Helpers;

public static class ValidationHelper
{
    public static IEnumerable<ValidationFailure> ToValidationFailures(Dictionary<string, IReadOnlyCollection<string>> errors)
    {
        return errors.SelectMany(kv => kv.Value.Select(msg => new ValidationFailure(kv.Key, msg)));
    }
}
