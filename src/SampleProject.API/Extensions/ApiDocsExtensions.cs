using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace SampleProject.API.Extensions;

public static class ApiDocsExtensions
{
    public static void AddApiDocs(this IServiceCollection services)
    {
        services.AddOpenApi();
    }

    public static void UseApiDocs(this WebApplication app, IConfiguration configuration)
    {
        var allowDocs = configuration.GetValue<bool>("ApiDocs:Enabled");
        if (!allowDocs)
        {
            return;
        }

        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.SortTagsAlphabetically();
        });
    }
}
