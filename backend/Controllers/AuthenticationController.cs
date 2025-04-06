using TaskSync.Models.Request;
using Microsoft.AspNetCore.Mvc;
using TaskSync.Services.Interfaces;
using TaskSync.ActionFilterAttributes;
using Microsoft.EntityFrameworkCore;
using TaskSync.Repositories;

namespace TaskSync.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Test")]
        [ValidateRequest]
        public async Task<IActionResult> Test([FromBody] TestRequest request)
        {
            Console.WriteLine("AuthenticationController/Test");
            
            var result = await _userService.GetUserAsync();

            return Ok(result);
        }
    }
}