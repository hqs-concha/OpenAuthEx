
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc.Core.Attribute;
using Sample.Model;
using Tools.Exception;

namespace Sample.Controllers
{
    [Route("test")]
    [ApiController, AllowAnonymous]
    public class TestController : ControllerBase
    {
        [HttpPost("list")]
        public IActionResult Index([FromBody] ValidateDto dto)
        {
            throw new CustomException("aaa");
        }

        [HttpPost]
        public IActionResult Test([FromForm]ValidateDto dto)
        {
            var name = HttpUtility.UrlEncode(dto.Id);
            return Ok(name);
        }

        [HttpPost("upload"), IgnoreAntiReplay]
        public IActionResult Upload([FromBody] IFormFile file)
        {
            return Ok();
        }
    }
}
