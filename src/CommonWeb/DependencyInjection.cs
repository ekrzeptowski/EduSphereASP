using CommonWeb.Infrastructure;
using CommonWeb.Services;
using EduSphere.Application.Common.Interfaces;
using EduSphere.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace CommonWeb;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, string applicationName)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.AddSecurity("Bearer",
                new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Description = "Wprowadź token JWT"
                });
            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
            configure.PostProcess = document =>
            {
                document.Info.Title = applicationName + " Documentation";
                document.Info.Version = "v1";
            };
        });

        return services;
    }
}
