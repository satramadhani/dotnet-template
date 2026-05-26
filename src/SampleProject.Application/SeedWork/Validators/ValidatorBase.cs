namespace SampleProject.Application.SeedWork.Validators;

public abstract class ValidatorBase<TRequest> : IRequestValidator<TRequest>
{
    private readonly Dictionary<string, List<string>> _errors = new();

    public Dictionary<string, IReadOnlyCollection<string>> Validate(TRequest request)
    {
        _errors.Clear();
        DoValidate(request);
        
        return _errors.ToDictionary(kv => kv.Key, IReadOnlyCollection<string> (kv) => kv.Value);
    }

    protected abstract void DoValidate(TRequest request);

    protected void AddError(string key, string message)
    {
        if (!_errors.TryGetValue(key, out var list))
        {
            list = [];
            _errors[key] = list;
        }

        list.Add(message);
    }
}
