namespace SampleProject.API.Shared.Responses;

public record ResponseBase(int Status, string? Message);

public record ResponseBase<TData>(int Status, string? Message, TData? Data);

public record ResponseBase<TData, TMeta>(int Status, string? Message, TData? Data, TMeta? Meta);
