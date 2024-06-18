using CommonWeb;
using EduSphere.Infrastructure.Data;
using EduSphere.Web;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using Yarp.ReverseProxy.Swagger;
using Yarp.ReverseProxy.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);
var reverseProxyConfig = builder.Configuration.GetSection("ReverseProxy");
// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices("Web gateway");
builder.Services.AddReverseProxy()
    .LoadFromConfig(reverseProxyConfig)
    .AddSwagger(reverseProxyConfig);
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    var config = app.Services.GetRequiredService<IOptionsMonitor<ReverseProxyDocumentFilterConfig>>().CurrentValue;
    foreach (var cluster in config.Clusters)
    {
        Console.WriteLine($"Adding Swagger UI for {cluster.Key}");
        settings.SwaggerRoutes.Add(new SwaggerUiRoute($"/api/{cluster.Key}", $"/api/{cluster.Key}/specification.json"));
    }

    settings.Path = "/api";
    settings.DocumentPath = "/api/{documentName}/specification.json";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler(
    new ExceptionHandlerOptions()
    {
        AllowStatusCode404Response = true, // important!
        ExceptionHandlingPath = "/error"
    });

app.MapReverseProxy();

app.Run();

public partial class Program
{
}
