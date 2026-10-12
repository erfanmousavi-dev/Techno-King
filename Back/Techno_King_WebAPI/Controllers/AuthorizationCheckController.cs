using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Techno_King_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationCheckController : ControllerBase
    {
        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth() => Ok(new { message = "You are authorized!" });
    }
}
