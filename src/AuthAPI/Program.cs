using CommonWeb;
using EduSphere.Infrastructure;
using EduSphere.Infrastructure.Data;
using NSwag.Generation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices("AuthAPI");

// change settings of AddOpenApiDocument
builder.Services.Configure<AspNetCoreOpenApiDocumentGeneratorSettings>(options => options.PostProcess = document =>
{
    document.Info.Title = "EduSphere AuthAPI Documentation";
    document.Info.Version = "v1";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
    app.UseSwaggerUi(settings =>
    {
        settings.Path = "/api";
        settings.DocumentPath = "/api/auth/specification.json";
    });
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseExceptionHandler(
    new ExceptionHandlerOptions()
    {
        AllowStatusCode404Response = true, // important!
        ExceptionHandlingPath = "/error"
    });

app.MapEndpoints();

app.Run();

public partial class Program
{
}
