using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [ApiVersion("2.0")]
    //[Route("api/v{v:apiVersion}/teste")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV2Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<html><body><h2>TesteV2Controller - V 2.0</h2> </body> </html>"
            };
        } 
        
    }
}