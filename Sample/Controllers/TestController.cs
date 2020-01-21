
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet, Authorize]
        public IActionResult Index()
        {
            return Ok(DateTime.UtcNow);
        }
    }
}