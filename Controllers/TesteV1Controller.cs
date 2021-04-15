using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    //[ApiVersion("1.0", Deprecated = true)]
    //[ApiVersion("2.0")]    
    //[Route("api/v{v:apiVersion}/teste")]
    [ApiVersion("1.0")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV1Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<html><body><h2>TesteV1Controller - V 1.0</h2> </body> </html>"
            };
        }        

        /*[HttpGet, MapToApiVersion("2.0")]
        public IActionResult GetVersao2()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<html><body><h2>GET TesteV1Controller - GET V 2.0</h2> </body> </html>"
            };
        }  */  
    }
}