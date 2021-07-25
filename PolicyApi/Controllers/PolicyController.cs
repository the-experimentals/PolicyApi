using Microsoft.AspNetCore.Mvc;


namespace PolicyApi.Controllers
{
    [Route("policy")]
    [ApiController]
    public class PolicyController : Controller
    {
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Testing policy api endpoint");
        }
    }
}
