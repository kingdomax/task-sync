using Microsoft.AspNetCore.Mvc;
using TaskSync.Models.Request;
using TaskSync.Models.Response;

namespace TaskSync.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        //private readonly IAuthenticationService _authenticationService;

        public AuthenticationController()
        {
            //_authenticationService = authenticationService;
        }

        [HttpPost("Test")]
        public IActionResult Test([FromBody] TestRequest request)
        {
            var result = request != null ? new TestResponse(request.Name + request.Id) : null;
            return Ok(result);
        }
    }
}