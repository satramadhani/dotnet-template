using SampleProject.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();

builder.Services.AddControllers();
builder.Services.AddApiDocs();

var app = builder.Build();

app.UseApiDocs(builder.Configuration);
app.UseExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
