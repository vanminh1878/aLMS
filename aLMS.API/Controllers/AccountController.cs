using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { Message = "Hello World" });
    }
}
