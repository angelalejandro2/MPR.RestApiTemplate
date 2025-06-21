using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MPR.RestApiTemplate.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class InfoController(IConfiguration configuration) : ControllerBase
    {
        public IConfiguration Configuration { get; } = configuration;

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult ApiInfo()
        {
            var info = new
            {
                name = "MPR.RestApiTemplate",
                version = "1.0",
                description = "REST API template",
                documentation = new
                {
                    scalar = $"{Request.Scheme}://{Request.Host}/scalar",
                    swagger = $"{Request.Scheme}://{Request.Host}/swagger"
                },
                domains = new[] {"Lorem Ipsum"}, // Reemplazar según corresponda
                serverTime = DateTime.UtcNow,
                message = "Customize this endpoint as needed."
            };

            return Ok(info);
        }

        [AllowAnonymous]
        [HttpGet("html")]
        [Produces("text/html")]
        public IActionResult ApiInfoHtml()
        {
            var html = $"""
            <html>
            <head>
                <title>MPR.RestApiTemplate API</title>
                <link href='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css' rel='stylesheet'>
            </head>
            <body>
                <div class='container mt-5'>
                    <div class='jumbotron'>
                        <h1 class='display-4'>MPR.RestApiTemplate API v1.0</h1>
                        <p class='lead'>Reusable REST API template. Customize this as needed.</p>
                        <hr class='my-4'>
                        <p>Explore the API documentation below:</p>
                        <a class='btn btn-primary btn-lg' href='/scalar' role='button'>Scalar Docs</a>
                        <a class='btn btn-outline-secondary btn-lg ml-2' href='/swagger' role='button'>Swagger UI</a>
                    </div>
                    <footer class='text-muted text-center'>
                        <small>Server time (UTC): {DateTime.UtcNow}</small>
                    </footer>
                </div>
            </body>
            </html>
            """;

            return Content(html, "text/html");
        }
    }
}