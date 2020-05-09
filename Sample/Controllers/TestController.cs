
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Core.Attribute;
using Sample.Model;
using Tools.Utils;

namespace Sample.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("list")]
        public async Task<IActionResult> Index([FromQuery] ValidateDto dto)
        {
            var result = await HttpHelper.GetStringAsync("http://account.hqs.pub");
            return Ok(result);
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
