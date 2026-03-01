using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(new[]
            {
                new { Id = 1, Username = "admin" },
                new { Id = 2, Username = "user" }
            });
        }
    }
}