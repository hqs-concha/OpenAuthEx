
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tools.Utils;

namespace Sample.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await HttpHelper.GetStringAsync("http://account.hqs.pub");
            return Ok(result);
        }
    }
}