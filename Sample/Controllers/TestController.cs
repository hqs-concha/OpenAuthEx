
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string code)
        {
            return Ok(code);
        }
    }
}