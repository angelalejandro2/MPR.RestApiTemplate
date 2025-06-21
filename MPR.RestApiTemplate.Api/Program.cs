using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using MPR.RestApiTemplate.Application.Mappings;
using MPR.RestApiTemplate.Application.Services;
using MPR.RestApiTemplate.Domain.Interfaces;
using MPR.RestApiTemplate.Infrastructure;
using MPR.RestApiTemplate.Infrastructure.Context;
using MPR.RestApiTemplate.Security.Authorization;
using MPR.RestApiTemplate.Security.Factories;
using MPR.RestApiTemplate.Security.Interfaces;
using Scalar.AspNetCore;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = ConfigureApp(builder);
        app.Run();
    }

    public static WebApplication ConfigureApp(WebApplicationBuilder builder)
    { 
        builder.Services.AddInfrastructureDbContexts(builder.Configuration);

        //mvc service (set to ignore ReferenceLoopHandling in json serialization like Users[0].Account.Users)
        builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
        .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.generated.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();


        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        builder.Services.AddApplicationServices();
        builder.Services.AddMappingProfiles();

        //API versioning service
        builder.Services.AddApiVersioning(
            o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                //versioning by url segment
                options.SubstituteApiVersionInUrl = true;
            });

        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<SecurityProviderFactory>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<SecurityProviderFactory>().CreateProvider());
        builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        builder.Services.AddAuthorization(options =>
        {
            var policies = builder.Configuration.GetSection("Security:Policies").GetChildren();
            foreach (var policy in policies)
            {
                options.AddPolicy(policy.Key, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new PermissionRequirement(policy.Key));
                });
            }
        });


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.Use(async (context, next) =>
        {
            var provider = context.RequestServices.GetRequiredService<ISecurityProvider>();

            // Autenticación
            var authenticated = await provider.AuthenticateAsync(context);
            if (!authenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            // Claims principal
            var username = context.User?.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(username))
            {
                var principal = await provider.CreateClaimsPrincipalAsync(username);
                context.User = principal;
            }

            await next();
        });

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async http =>
            {
                http.Response.Redirect("api/info/html", true);
            });
            endpoints.MapGet("/api", async http =>
            {
                http.Response.Redirect("api/info/html", true);
            });
        });

        app.MapControllers();

        return app;
    }

}