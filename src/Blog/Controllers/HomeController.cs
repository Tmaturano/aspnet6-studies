using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet("health-check")]
        [ApiKey]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
