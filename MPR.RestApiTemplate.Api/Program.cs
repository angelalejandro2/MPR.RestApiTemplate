using Asp.Versioning;
using MPR.RestApiTemplate.Application.Mappings;
using MPR.RestApiTemplate.Application.Services;
using MPR.RestApiTemplate.Domain.Interfaces;
using MPR.RestApiTemplate.Infrastructure;
using MPR.RestApiTemplate.Infrastructure.Context;
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
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async http =>
            {
                http.Response.Redirect("api/info", true);
            });
            endpoints.MapGet("/api", async http =>
            {
                http.Response.Redirect("api/info", true);
            });
        });

        app.MapControllers();

        return app;
    }

}