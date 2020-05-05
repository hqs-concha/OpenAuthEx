
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sample.Model;
using Tools.Utils;

namespace Sample.Controllers
{
    [Route("client")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost("list")]
        public async Task<IActionResult> Index(ValidateDto dto)
        {
            var result = await HttpHelper.GetStringAsync("http://account.hqs.pub");
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Test([FromQuery]ValidateDto dto)
        {
            throw new Exception("asdasd");
            return Ok();
        }
    }
}