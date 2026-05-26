using FluentValidation;

namespace SampleProject.Application.SeedWork.Validators;

public abstract class FluentValidatorBase<TRequest> : AbstractValidator<TRequest>, IRequestValidator<TRequest>
{
    public new Dictionary<string, IReadOnlyCollection<string>> Validate(TRequest request)
    {
        var result = base.Validate(request);
        if (result.IsValid)
        {
            return [];
        }

        return result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                IReadOnlyCollection<string> (g) => g.Select(e => e.ErrorMessage).ToList());
    }
}
