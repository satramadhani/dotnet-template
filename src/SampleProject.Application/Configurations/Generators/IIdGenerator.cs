namespace SampleProject.Application.Configurations.Generators;

// TODO: Refactor to IIdGenerator<T> to support alternate ID types (e.g., Guid, string).
public interface IIdGenerator
{
    long Generate();

    IReadOnlyList<long> GenerateBatch(int size);
    
    DateTimeOffset ExtractTimestamp(long id);
}
