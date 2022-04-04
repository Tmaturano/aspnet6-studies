using Microsoft.AspNetCore.Mvc;

namespace TodoMVC.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public string Get()
        {
            return "Hello World";
        }
    }
}
