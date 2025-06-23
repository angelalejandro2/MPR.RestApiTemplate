using Asp.Versioning;
using MPR.RestApiTemplate.Api.Security.Extensions;
using MPR.RestApiTemplate.Application.Mappings;
using MPR.RestApiTemplate.Application.Services;
using MPR.RestApiTemplate.Domain.Interfaces;
using MPR.RestApiTemplate.Infrastructure;
using MPR.RestApiTemplate.Infrastructure.Context;

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
        builder.Services.AddControllers()
        .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });

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

        builder.Services.AddJwtSecurity(builder.Configuration);

        // Configure authorization policies
        builder.Services.AddAuthorization(options =>
        {
            var policies = builder.Configuration.GetSection("Security:Policies").GetChildren();
            foreach (var policy in policies)
            {
                options.AddPolicy(policy.Key, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new MPR.RestApiTemplate.Api.Security.Authorization.PermissionRequirement(policy.Key));
                });
            }
        });


        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApiDocument(config => {
            config.Title = "Rest APi Template";
            config.Version = "v1";
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Route mappings
        app.MapGet("/", () => Results.Redirect("api/info/html", true));
        app.MapGet("/api", () => Results.Redirect("api/info/html", true));
        app.MapControllers();

        return app;
    }
}