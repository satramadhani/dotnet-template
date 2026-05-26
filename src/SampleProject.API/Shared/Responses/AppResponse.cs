namespace SampleProject.API.Shared.Responses;

public static class AppResponse
{
    public static ResponseBase<TData> Ok<TData>(TData data, string message = "Success", int status = 200)
    {
        return new ResponseBase<TData>(status, message, data);
    }

    public static ResponseBase<TData, TMeta> Ok<TData, TMeta>(TData data, TMeta? meta, string message = "Success",
        int status = 200)
    {
        return new ResponseBase<TData, TMeta>(status, message, data, meta);
    }

    public static ResponseBase Created(string message = "Created", int status = 201)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase<TData> Created<TData>(TData? data, string message = "Created", int status = 201)
    {
        return new ResponseBase<TData>(status, message, data);
    }

    public static ResponseBase<TData, TMeta> Created<TData, TMeta>(TData? data, TMeta? metadata, string message = "Created",
        int status = 201)
    {
        return new ResponseBase<TData, TMeta>(status, message, data, metadata);
    }

    public static ResponseBase NoContent(string message = "No Content", int status = 204)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase<object, TMeta> NoContent<TMeta>(TMeta? metadata, string message = "No Content",
        int status = 204)
    {
        return new ResponseBase<object, TMeta>(status, message, null, metadata);
    }

    public static ResponseBase BadRequest(string message = "Bad Request", int status = 400)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase<TData> BadRequest<TData>(TData? data, string message = "Bad Request", int status = 400)
    {
        return new ResponseBase<TData>(status, message, data);
    }

    public static ResponseBase<TData, TMeta> BadRequest<TData, TMeta>(TData? data, TMeta? metadata,
        string message = "Bad Request", int status = 400)
    {
        return new ResponseBase<TData, TMeta>(status, message, data, metadata);
    }

    public static ResponseBase Unauthorized(string message = "Unauthorized", int status = 401)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase Forbidden(string message = "Forbidden", int status = 403)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase NotFound(string message = "Not Found", int status = 404)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase<TData> NotFound<TData>(TData? data, string message = "Not Found", int status = 404)
    {
        return new ResponseBase<TData>(status, message, data);
    }

    public static ResponseBase<TData, TMeta> NotFound<TData, TMeta>(TData? data, TMeta? metadata, string message = "Not Found",
        int status = 404)
    {
        return new ResponseBase<TData, TMeta>(status, message, data, metadata);
    }

    public static ResponseBase Conflict(string message = "Conflict", int status = 409)
    {
        return new ResponseBase(status, message);
    }

    public static ResponseBase Error(string message = "Internal Server Error", int status = 500)
    {
        return new ResponseBase(status, message);
    }
}
