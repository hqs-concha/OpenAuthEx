
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Core.Attribute;
using Sample.Model;
using Tools.Exception;
using Tools.Utils;

namespace Sample.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("list"), Authorize]
        public IActionResult Index([FromQuery] ValidateDto dto)
        {
            throw new CustomException("aaa");
        }

        [HttpPost, IgnoreVerify]
        public IActionResult Test([FromQuery]ValidateDto dto)
        {
            return Ok(dto);
        }

        [HttpPost("upload"), IgnoreAntiReplay]
        public IActionResult Upload([FromBody] IFormFile file)
        {
            return Ok();
        }
    }
}
