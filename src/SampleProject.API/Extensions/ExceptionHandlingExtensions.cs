using Microsoft.AspNetCore.Diagnostics;
using SampleProject.API.Shared.Responses;
using SampleProject.Application.Shared.Exceptions;

namespace SampleProject.API.Extensions;

public static class ExceptionHandlingExtensions
{
    public static void UseExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(config =>
        {
            config.Run(async context =>
            {
                const string contentType = "application/json";
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                switch (exception)
                {
                    case ValidationException validation:
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = contentType;
                        
                        var response = AppResponse.BadRequest(data: validation.Errors, message: validation.Message);
                        await context.Response.WriteAsJsonAsync(response);
                        break;
                    }
                    default:
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = contentType;

                        var exceptionMessage = exception?.InnerException?.Message ?? exception?.Message;
                        const string defaultMessage = "An error occurred while trying to process your request.";

                        var response = app.Environment.IsDevelopment()
                            ? AppResponse.Error(exceptionMessage ?? defaultMessage)
                            : AppResponse.Error(defaultMessage);

                            await context.Response.WriteAsJsonAsync(response);
                            break;
                    }
                }
            });
        });
    }
}
