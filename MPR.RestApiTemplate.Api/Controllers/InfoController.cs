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

        //get info
        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        public IActionResult ApiInfo()
        {
            return Content("<html><head><link href='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh' crossorigin='anonymous'></head><body>" +

                "<div class='jumbotron'>" +
                "<h1><i class='fab fa-centercode' fa-2x></i>  MPR.RestApiTemplate Api v.1</h1>" +
                "<h4>Lorem Ipsum</h4>" +
                "<br/>" +
                 "<a class='btn btn-outline-primary' role='button' href='/scalar'><b>API specifications</b></a><br>" +
                "</div>" +
                "<div class='row'>" +
                "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>" +
                "</div>" +
                "</body></html>"
               , "text/html");

        }

    }
}
